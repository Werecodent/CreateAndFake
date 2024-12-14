using System.Collections.Frozen;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.RandomizerTool;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFake.Toolbox.MutatorTool;

/// <summary>Configuration for controlling mutating behavior.</summary>
public record MutatorOptions : IToolOptions
{
    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Ensures object variance.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Limits attempts at creating variants.</summary>
    public Limiter Limiter { get; init; } = Limiter.Score;

    /// <summary>Types with too small of range for unique randomization.</summary>
    public FrozenSet<Type> UniqueIgnoredTypes { get; init; } = FrozenSet.ToFrozenSet([
        typeof(bool),
        typeof(byte),
        typeof(char)]);
}