using System.Collections.Frozen;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Engine;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.RandomizerTool;

/// <summary>Configuration for controlling randomization behavior.</summary>
public sealed record RandomizerOptions : ToolHintOptions<RandomizerOptions, CreateHint>
{
    /// <summary>Value generator used for base randomization.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Provides stubs.</summary>
    public required IFaker Faker { get; init; }

    /// <summary>Limits attempts at matching conditions.</summary>
    [ConfigurableOption]
    public Limiter RandomizerCreateAttempts { get; init; } = Limiter.Score;

    /// <summary>Limits attempts at creating unspecific objects.</summary>
    [ConfigurableOption]
    public Limiter ObjectCreateAttempts { get; init; } = Limiter.Dozen;

    /// <summary>Limits population attempts for collections when encountering problems.</summary>
    [ConfigurableOption]
    public Limiter CollectionAttempts { get; init; } = Limiter.Score;

    /// <summary>Inclusive minimum size for created collections.</summary>
    [ConfigurableOption]
    public int CollectionMinSize { get; init; } = 1;

    /// <summary>Inclusive maximum size for created collections.</summary>
    [ConfigurableOption]
    public int CollectionMaxSize { get; init; } = 3;

    /// <summary>Inclusive minimum size for created strings.</summary>
    [ConfigurableOption]
    public int StringMinSize { get; init; } = 7;

    /// <summary>Inclusive maximum size for created string.</summary>
    [ConfigurableOption]
    public int StringMaxSize { get; init; } = 12;

    /// <summary>Characters to include in random strings.</summary>
    [ConfigurableOption]
    public FrozenSet<char> StringCharacterSet { get; init; } =
        FrozenSet.ToFrozenSet("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");

    /// <summary>Condition for the resulting randomized instance to match.</summary>
    public Func<object, bool>? FinalCondition { get; init; } = null;

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal RandomizerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Randomizer));
        if (section == null)
        {
            return this;
        }

        return this with
        {
            RandomizerCreateAttempts = section.GetValue(
                nameof(RandomizerCreateAttempts),
                RandomizerCreateAttempts
            ),
            ObjectCreateAttempts = section.GetValue(
                nameof(ObjectCreateAttempts),
                ObjectCreateAttempts
            ),
            CollectionAttempts = section.GetValue(nameof(CollectionAttempts), CollectionAttempts),
            CollectionMinSize = section.GetValue(nameof(CollectionMinSize), CollectionMinSize),
            CollectionMaxSize = section.GetValue(nameof(CollectionMaxSize), CollectionMaxSize),
            StringMinSize = section.GetValue(nameof(StringMinSize), StringMinSize),
            StringMaxSize = section.GetValue(nameof(StringMaxSize), StringMaxSize),
            StringCharacterSet = section.GetValue(nameof(StringCharacterSet), StringCharacterSet),
        };
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(
            Gen,
            Faker,
            RandomizerCreateAttempts,
            ObjectCreateAttempts,
            CollectionAttempts,
            CollectionMinSize,
            CollectionMaxSize,
            StringMinSize,
            StringMaxSize,
            StringCharacterSet,
            IncludeDefaultHints,
            Hints,
            FinalCondition
        );
    }

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
