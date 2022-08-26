using System.Diagnostics;

namespace Brainstorm.Rig.Services;
public class ProcessRunner : IDisposable
{
    readonly Process process;

    bool CheckProcess => Process.GetProcessesByName(process?.ProcessName).Any();

    public bool? Running { get; private set; } = null;

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

    DataReceivedEventHandler ProcessOutput =>
        new((sender, e) =>
        {
            if (!Running.HasValue && e.Data.Contains("Now listening on: http://localhost:5000"))
                Running = true;

            Console.WriteLine(e.Data);
        });

    DataReceivedEventHandler ProcessError =>
        new((sender, e) =>
        {
            Running = CheckProcess;
            Console.WriteLine(e.Data);
        });

    public ProcessRunner(string connection)
    {
        process = new()
        {
            StartInfo = GetConfiguration(connection)
        };

        process.OutputDataReceived += ProcessOutput;
        process.ErrorDataReceived += ProcessError;
    }

    public bool Start()
    {
        var res = process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (res)
            while (!Running.HasValue) { }

        return res;
    }

    public void Dispose()
    {
        if (Running.Value)
            process.Kill();

        process.Dispose();
        GC.SuppressFinalize(this);
    }
}