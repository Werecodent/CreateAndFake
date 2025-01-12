using static Bullseye.Targets;
using static SimpleExec.Command;

namespace Build;

/// <summary>Manages build behavior for the solution.</summary>
internal static class Program
{
    /// <summary>Base directory for all output.</summary>
    private static readonly string _ArtifactDir = Path.Combine(Directory.GetCurrentDirectory(), "artifacts");

    /// <summary>Console application entry point.</summary>
    /// <param name="args">Command-line arguments.</param>
    public static async Task Main(string[] args)
    {
        Target("default", DependsOn("coverage"));
        Target("restore", Restore);
        Target("compile", DependsOn("restore"), Compile);
        Target("test", DependsOn("compile"), Test);
        Target("coverage", DependsOn("compile"), Coverage);
        Target("pack", DependsOn("compile"), Pack);
        Target("debugCrash", DependsOn("compile"), DebugCrash);
        await RunTargetsAndExitAsync(args);
    }

    /// <summary>Downloads all packages for the solution.</summary>
    private static Task Restore()
    {
        return RunAsync("dotnet", "restore");
    }

    /// <summary>Builds the solution.</summary>
    private static async Task Compile()
    {
        await RunAsync($"dotnet", $"build --no-restore --configuration Debug");
        await RunAsync($"dotnet", $"build --no-restore --configuration Release");
    }

    /// <summary>Tests the solution.</summary>
    private static async Task Test()
    {
        string testArgs = "test --no-restore --no-build ";

        await RunAsync("dotnet", testArgs + "--configuration Debug");
        await RunAsync("dotnet", testArgs + "--configuration Release");
    }

    /// <summary>Uses extended logging for tests to check on harness crash.</summary>
    private static async Task DebugCrash()
    {
        string logDir = Path.Combine(_ArtifactDir, "logs");
        EnsureEmpty(logDir);

        string logFile = Path.Combine(logDir, "test.txt");
        string testArgs = $"test --no-restore --no-build --diag:{logFile} ";

        await RunAsync("dotnet", testArgs + "--configuration Debug");
    }

    /// <summary>Tests and analyzes test code coverage.</summary>
    private static async Task Coverage()
    {
        string prefix = "coverage";
        string postfix = ".cobertura.xml";

        string toolsDir = Path.Combine(_ArtifactDir, "tools");
        string coverageDir = Path.Combine(_ArtifactDir, "coverage");
        string testDir = Path.Combine(coverageDir, "testResults");
        string reportDir = Path.Combine(coverageDir, "report");

        EnsureEmpty(coverageDir);

        await RunAsync("dotnet", string.Join(' ',
            "test",
            "--no-build",
            "--no-restore",
            "--configuration Debug",
            "--collect:\"XPlat Code Coverage\"",
            $"--results-directory \"{testDir}\""));

        int count = 0;
        foreach (string result in Directory.GetFiles(testDir, $"{prefix}{postfix}", SearchOption.AllDirectories))
        {
            File.Copy(result, Path.Combine(coverageDir, $"{prefix}{count++}{postfix}"));
        }

        await RunAsync("dotnet", $"tool update dotnet-reportgenerator-globaltool --tool-path {toolsDir}");
        await RunAsync($"{toolsDir}/reportgenerator", $"-reports:{coverageDir}/*.xml -targetdir:{reportDir}");
    }

    /// <summary>Packs the solution.</summary>
    private static Task Pack()
    {
        string releaseDir = Path.Combine(_ArtifactDir, "releases");
        EnsureEmpty(releaseDir);

        return RunAsync("dotnet", string.Join(' ',
            "pack",
            "--no-build",
            "--no-restore",
            "--configuration Release",
            $"--output \"{releaseDir}\""));
    }

    /// <summary>Deletes and creates a directory.</summary>
    /// <param name="dir">Directory to empty.</param>
    private static void EnsureEmpty(string dir)
    {
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }
        _ = Directory.CreateDirectory(dir);
    }
}