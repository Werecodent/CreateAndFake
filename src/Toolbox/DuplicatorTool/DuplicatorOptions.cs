using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.DuplicatorTool;

/// <summary>Configuration for controlling duplication behavior.</summary>
public record DuplicatorOptions : IToolOptions
{
    /// <summary>Verifies duplicates are valid.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Finds contents for objects.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>If the default set of hints should be used in duplication.</summary>
    public bool IncludeDefaultHints { get; init; } = true;

    /// <summary>If results are verified via the <see cref="Asserter"/>.</summary>
    public bool VerifyCloneResult { get; init; } = true;

    /// <summary>Custom duplicators used to deep copy specific types.</summary>
    public ImmutableArray<CopyHint> Hints { get; init; } = [];

    /// <summary>Types that need no further inspection for serialization/deserialization.</summary>
    public FrozenSet<Type> SerializableTypes { get; init; } =
        FrozenSet.ToFrozenSet([typeof(string), typeof(Type)]);
}
