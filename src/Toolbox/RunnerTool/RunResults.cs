using System.Text;
using Werecodent.CreateAndFake.Design;

namespace Werecodent.CreateAndFake.RunnerTool;

/// <summary>Results for a series of associated calls.</summary>
/// <param name="rawResults"><inheritdoc cref="RawResults" path="/summary"/></param>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
public sealed class RunResults(IEnumerable<RunResult> rawResults, RunnerOptions options)
{
    /// <summary>Associated calls with results.</summary>
    public IEnumerable<RunResult> RawResults { get; } = [.. rawResults];

    /// <summary>Configured options applied to the associated calls.</summary>
    private RunnerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder contents = new();

        int i = 0;
        foreach (object item in RawResults)
        {
            ArgumentGuard.ThrowUponIterationLimit(i, Options.Valuer.Options.IterationLimit);
            _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine();
        }
        return contents.ToString();
    }
}
