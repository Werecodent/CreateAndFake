using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.RandomizerTool;

namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Configuration for controlling automated testing behavior.</summary>
public record TesterOptions : IToolOptions
{
    /// <summary>Core value random handler.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Creates objects and populates them with random values.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Deep clones objects.</summary>
    public required IDuplicator Duplicator { get; init; }

    /// <summary>Handles common test scenarios.</summary>
    public required Asserter Asserter { get; init; }

    /// <summary>Retries tests if timeout is reached.</summary>
    public Limiter Limiter { get; init; } = Limiter.Dozen;

    /// <summary>How long to wait for tests to complete.</summary>
    public TimeSpan Timeout { get; init; } = new(0, 0, 3);
}