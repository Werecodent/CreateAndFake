using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.FakerTool;

namespace CreateAndFake.RandomizerTool;

/// <summary>Configuration for controlling randomization behavior.</summary>
public record RandomizerOptions : IToolOptions
{
    /// <summary>Value generator used for base randomization.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Provides stubs.</summary>
    public required IFaker Faker { get; init; }

    /// <summary>Limits attempts at matching conditions.</summary>
    public Limiter Limiter { get; init; } = Limiter.Score;

    /// <summary>Limits population attempts for collections when encountering problems.</summary>
    public Limiter CollectionAttempts { get; init; } = Limiter.Score;

    /// <summary>Inclusive minimum size for created collections.</summary>
    public int CollectionMinSize { get; init; } = 1;

    /// <summary>Inclusive maximum size for created collections.</summary>
    public int CollectionMaxSize { get; init; } = 3;

    /// <summary>Inclusive minimum size for created strings.</summary>
    public int StringMinSize { get; init; } = 7;

    /// <summary>Inclusive maximum size for created string.</summary>
    public int StringMaxSize { get; init; } = 12;

    /// <summary>Characters to include in random strings.</summary>
    public FrozenSet<char> StringCharacterSet { get; init; } =
        FrozenSet.ToFrozenSet(@"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");

    /// <summary>If the default set of hints should be used in randomization.</summary>
    public bool IncludeDefaultHints { get; init; } = true;

    /// <summary>Custom generators used to randomize specific types.</summary>
    public ImmutableArray<CreateHint> Hints { get; init; } = [];

    /// <summary>Condition for the resulting randomized instance to match.</summary>
    public Func<object, bool>? FinalCondition { get; init; } = null;

    /// <summary>Options to use when randomizing child values.</summary>
    public RandomizerOptions? NestedOptions { get; init; } = null;

    /// <inheritdoc cref="NextSize"/>
    /// <seealso cref="CollectionMinSize"/>
    /// <seealso cref="CollectionMaxSize"/>
    public int NextCollectionSize()
    {
        return NextSize(CollectionMinSize, CollectionMaxSize);
    }

    /// <inheritdoc cref="NextSize"/>
    /// <seealso cref="StringMinSize"/>
    /// <seealso cref="StringMaxSize"/>
    public int NextStringSize()
    {
        return NextSize(StringMinSize, StringMaxSize);
    }

    /// <summary>Generates a random length within configured bounds.</summary>
    /// <param name="setMin">Inclusive minimum size.</param>
    /// <param name="setMax">Inclusive maximum size.</param>
    /// <returns>The next size to use.</returns>
    private int NextSize(int setMin, int setMax)
    {
        int min,
            max;
        if (setMin <= setMax)
        {
            min = setMin;
            max = setMax;
        }
        else
        {
            min = max = setMax;
        }

        if (max == int.MaxValue)
        {
            max -= 1;
        }
        return Math.Max(0, Gen.Next(min, max + 1));
    }
}
