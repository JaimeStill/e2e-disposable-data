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
        ResetDbManager().Wait();
        State = InitState();
    }

    async Task Broadcast (string message, bool isError = false) =>
        await socket
            .Clients
            .All
            .SendAsync("output", new RigOutput { Message = message, IsError = isError });

    DataReceivedEventHandler ProcessOutput =>
        async (sender, e) => await Broadcast(e.Data );

    DataReceivedEventHandler ProcessError =>
        async (sender, e) => await Broadcast(e.Data, true);

    EventHandler ProcessExit =>
        async (sender, e) => await Broadcast("Process exitied");

    EventHandler<DataSeededEventArgs> ProcessDataSeed =>
        async (sender, e) => await Broadcast(e.Message);

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

    async Task ResetDbManager(bool dispose = false)
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
            await StartProcess();
    }

    public async Task<RigState> InitializeDatabase()
    {
        await Broadcast($"Initializing the database at {manager.Connection}...");

        var result = await manager.InitializeAsync();
        State.DatabaseCreated = result;

        if (result) {
            Seeder = new(manager.Context);
            Seeder.DataSeeded += ProcessDataSeed;
            await Broadcast("Database successfully initialized");
        }

        return State;
    }

    public async Task<RigState> DestroyDatabase()
    {
        await Broadcast("Destroying the database...");
        await ResetDbManager(true);

        State.DatabaseCreated = false;
        State.Connection = manager.Connection;
        await Broadcast("Database successfully destroyed");

        return State;
    }

    public async Task<RigState> StartProcess()
    {
        runner.Start();
        await Broadcast($"Process Started: {runner.Process.ProcessName} - {runner.Process.Id}");
        State.ProcessRunning = runner.Running;
        return State;
    }

    public async Task<RigState> KillProcess()
    {
        await Broadcast($"Killing process {runner.Process.ProcessName} - {runner.Process.Id}...");
        runner.Kill();
        State.ProcessRunning = runner.Running;
        await Broadcast("Process successfully killed");
        return State;
    }

    public void Dispose()
    {
        manager.Dispose();
        runner.Dispose();
        GC.SuppressFinalize(this);
    }
}