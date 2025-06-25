using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.DuplicatorTool;

/// <summary>Configuration for controlling duplication behavior.</summary>
public record DuplicatorOptions : IToolHintOptions<DuplicatorOptions, CopyHint>
{
    /// <summary>Verifies duplicates are valid.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Finds contents for objects.</summary>
    public required IExtractor Extractor { get; init; }

    /// <inheritdoc/>
    public bool IncludeDefaultHints { get; init; } = true;

    /// <inheritdoc/>
    public ImmutableArray<CopyHint> Hints { get; init; } = [];

    /// <inheritdoc/>
    public DuplicatorOptions? NestedOptions => null;

    /// <summary>If results are verified via the <see cref="Asserter"/>.</summary>
    public bool VerifyCloneResult { get; init; } = true;

    /// <summary>Types that need no further inspection for serialization/deserialization.</summary>
    public FrozenSet<Type> SerializableTypes { get; init; } =
        FrozenSet.ToFrozenSet([typeof(string), typeof(Type)]);
}
