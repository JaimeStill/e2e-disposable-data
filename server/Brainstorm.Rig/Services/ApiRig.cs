using Brainstorm.Data;
using Brainstorm.Rig.Models;

namespace Brainstorm.Rig.Services;
public class ApiRig : IDisposable
{
    DbManager manager;
    ProcessRunner runner;
    public Seeder Seeder { get; private set; }
    public RigState State { get; private set; }

    public ApiRig()
    {
        ResetDbManager();
        State = InitState();
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
            runner.Kill();
        }

        manager = new("App", true, true);
        runner = new(manager.Connection);

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

    public async Task<RigState> DestroyDatabase()
    {
        var result = await manager.Destroy();

        if (result)
            ResetDbManager(true);

        State.Connection = manager.Connection;

        return State;
    }

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