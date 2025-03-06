using System.Collections.Frozen;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.ExtractorTool;

/// <summary>Configuration for controlling extraction behavior.</summary>
public record ExtractorOptions : IToolOptions
{
    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Ensures object variance.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Limits attempts at creating variants.</summary>
    public Limiter Limiter { get; init; } = Limiter.Score;

    /// <summary>If private properties/fields should be extracted as well.</summary>
    public bool ExtractPrivateMembers { get; init; } = false;

    /// <summary>Types with too small of range for unique randomization.</summary>
    public FrozenSet<Type> UniqueIgnoredTypes { get; init; } =
        FrozenSet.ToFrozenSet([typeof(bool), typeof(byte), typeof(char)]);

    /// <summary>Types that need no further inspection when creating a <see cref="ContentMap"/>.</summary>
    public FrozenSet<Type> ContentEndTypes { get; init; } = FrozenSet.ToFrozenSet<Type>([]);
}
