using Brainstorm.Data;

namespace Brainstorm.Rig.Services;
public class ApiRig : IDisposable
{
    readonly DbManager manager;
    readonly ProcessRunner runner;
    public Seeder Seeder { get; private set; }

    public ApiRig()
    {
        manager = new("App", true, true);
        runner = new(Connection);
        Seeder = new(manager.Context);
    }

    public string Connection => manager.Connection;
    public Task<bool> InitializeDatabase() => manager.InitializeAsync();
    public bool StartProcess() => runner.Start();

    public void Dispose()
    {
        manager.Dispose();
        runner.Dispose();
        GC.SuppressFinalize(this);
    }
}