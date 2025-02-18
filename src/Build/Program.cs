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
    public static Task Main(string[] args)
    {
        string[] configurations = ["Debug", "Release"];
        Target("default", DependsOn("coverage"));
        Target("compile", ForEach(configurations), Compile);
        Target("test", DependsOn("compile"), ForEach(configurations), Test);
        Target("coverage", DependsOn("compile"), Coverage);
        Target("pack", DependsOn("compile"), Pack);
        Target("debugCrash", DependsOn("compile"), DebugCrash);
        return RunTargetsAndExitAsync(args);
    }

    /// <summary>Builds the solution.</summary>
    /// <param name="configuration">Build configuration to use.</param>
    private static Task Compile(string configuration)
    {
        return RunAsync($"dotnet", $"build --configuration {configuration}");
    }

    /// <summary>Tests the solution.</summary>
    /// <param name="configuration">Build configuration to use.</param>
    private static Task Test(string configuration)
    {
        return RunAsync("dotnet", $"test --no-restore --no-build --configuration {configuration}");
    }

    /// <summary>Uses extended logging for tests to check on harness crash.</summary>
    private static Task DebugCrash()
    {
        string logDir = Path.Combine(_ArtifactDir, "logs");
        EnsureEmpty(logDir);

        string logFile = Path.Combine(logDir, "test.txt");
        string testArgs = $"test --no-restore --no-build --diag:{logFile} ";

        return RunAsync("dotnet", testArgs + "--configuration Debug");
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