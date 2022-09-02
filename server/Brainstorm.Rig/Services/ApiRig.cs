using System.Diagnostics;
using Brainstorm.Data;
using Brainstorm.Rig.Hubs;
using Brainstorm.Rig.Models;
using Microsoft.AspNetCore.SignalR;

namespace Brainstorm.Rig.Services;
public class ApiRig : IDisposable
{
    DbManager manager;
    ProcessRunner runner;
    readonly IHubContext<RigHub> socket;
    public Seeder Seeder { get; private set; }
    public RigState State { get; private set; }

    public ApiRig(IHubContext<RigHub> socket)
    {
        this.socket = socket;
        ResetDbManager();
        State = InitState();
    }

    static RigOutput CreateMessage(string message, RigState state, bool error = false, bool exiting = false) =>
        new()
        {
            Exiting = exiting,
            Output = new RigMessage
            {
                IsError = error,
                Message = message
            },
            State = state
        };

    DataReceivedEventHandler ProcessOutput =>
        new(async (sender, e) => await socket.Clients.All.SendAsync("output", CreateMessage(e.Data, State)));

    DataReceivedEventHandler ProcessError =>
        new(async (sender, e) => await socket.Clients.All.SendAsync("output", CreateMessage(e.Data, State, true)));

    EventHandler ProcessExit =>
        new(async (sender, e) => await socket.Clients.All.SendAsync("output", CreateMessage("Process exitied", State, false, true)));

    void RegisterProcessStreams()
    {
        runner.Process.OutputDataReceived += ProcessOutput;
        runner.Process.ErrorDataReceived += ProcessError;
        runner.Process.Exited += ProcessExit;
    }

    RigState InitState() => new()
    {
        Connection = manager.Connection,
        DatabaseCreated = false,
        ProcessRunning = false        
    };

    void ResetDbManager(bool dispose = false)
    {
        bool restartProcess = State is not null
            && State.ProcessRunning;

        if (dispose) {
            runner.Dispose();
            manager.Dispose();
        }

        manager = new("App", true, true);
        runner = new(manager.Connection);
        RegisterProcessStreams();

        if (restartProcess)
            StartProcess();
    }

    public async Task<RigState> InitializeDatabase()
    {
        var result = await manager.InitializeAsync();
        State.DatabaseCreated = result;

        if (result)            
            Seeder = new(manager.Context);

        return State;
    }

    public Task<RigState> DestroyDatabase() => Task.Run(() =>
    {
        ResetDbManager(true);

        State.DatabaseCreated = false;
        State.Connection = manager.Connection;

        return State;
    });

    public RigState StartProcess()
    {
        runner.Start();
        State.ProcessRunning = runner.Running;
        return State;
    }

    public RigState KillProcess()
    {
        runner.Kill();
        State.ProcessRunning = runner.Running;
        return State;
    }

    public void Dispose()
    {
        manager.Dispose();
        runner.Dispose();
        GC.SuppressFinalize(this);
    }
}