using System.Diagnostics;

namespace Brainstorm.Rig.Services;
public class ProcessRunner : IDisposable
{
    readonly Process process;
    bool disposed = false;

    public bool Running { get; private set; } = false;

    static ProcessStartInfo GetConfiguration(string connection) =>
        new()
        {
            FileName = "dotnet",
            Arguments = $"run --project \"..\\Brainstorm.Api\" /ConnectionStrings:App=\"{connection}\"",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

    void EndProcess()
    {
        if (disposed)
            return;

        Kill();
        process.Dispose();

        disposed = true;
    }

    DataReceivedEventHandler ProcessOutput =>
        new((sender, e) =>
        {
            if (!Running && e.Data.Contains("Now listening on: http://localhost:5000"))
                Running = true;

            Console.WriteLine(e.Data);
        });

    DataReceivedEventHandler ProcessError =>
        new((sender, e) =>
        {
            Running = !process.HasExited;
            Console.WriteLine(e.Data);
        });

    EventHandler ProcessExit =>
        new ((sender, e) => Running = false);

    public ProcessRunner(string connection)
    {
        process = new()
        {
            StartInfo = GetConfiguration(connection)
        };

        process.OutputDataReceived += ProcessOutput;
        process.ErrorDataReceived += ProcessError;
        process.Exited += ProcessExit;
    }

    ~ProcessRunner()
    {
        EndProcess();
    }

    public bool Start()
    {
        Kill();

        var res = process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (res)
            while (!Running) { }

        return res;
    }

    public bool Kill()
    {
        try
        {
            process.CancelOutputRead();
            process.CancelErrorRead();

            if (Running)
                process.Kill();

            Running = false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        EndProcess();
        GC.SuppressFinalize(this);
    }
}