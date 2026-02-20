namespace CreateAndFake.RunnerTool;

/// <summary>...</summary>
/// <param name="rawResults"><inheritdoc cref="RawResults" path="/summary"/></param>
public sealed class RunResults(IEnumerable<RunResult> rawResults)
{
    /// <summary>Associated method.</summary>
    public IEnumerable<RunResult> RawResults { get; } = [.. rawResults];
}
