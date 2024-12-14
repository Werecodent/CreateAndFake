using System.Collections.Immutable;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.AsserterTool;

namespace CreateAndFake.Toolbox.DuplicatorTool;

/// <summary>Configuration for controlling duplication behavior.</summary>
public record DuplicatorOptions : IToolOptions
{
    /// <summary>Verifies duplicates are valid.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>If the default set of hints should be used in duplication.</summary>
    public bool IncludeDefaultHints { get; init; } = true;

    /// <summary>Custom duplicators used to deep copy specific types.</summary>
    public ImmutableArray<CopyHint> Hints { get; init; } = [];
}
