using Brainstorm.Data;

namespace Brainstorm.Rig.Services;
public class ApiRig : IDisposable
{
    readonly DbManager manager;
    readonly ProcessRunner runner;

    public ApiRig()
    {
        manager = new("App", true, true);
        runner = new(Connection);
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