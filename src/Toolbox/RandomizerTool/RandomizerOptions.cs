using System.Collections.Frozen;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Properties;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool;

/// <summary>Configuration for controlling randomization behavior.</summary>
public sealed record RandomizerOptions : ToolHintOptions<RandomizerOptions, ICreateHint>
{
    /// <summary>Provides stubs.</summary>
    public required IFaker Faker { get; init; }

    /// <summary>Limits attempts at matching conditions.</summary>
    [ConfigurableOption]
    public Limiter RandomizerCreateAttempts { get; init; } = Limiter.Dozen;

    /// <summary>
    ///     When generating random objects, subclasses from the originating assembly are preferred
    ///     if  <see langword="true"/>. Otherwise, subclasses from all loaded assemblies are used.
    /// </summary>
    [ConfigurableOption]
    public bool PreferLocalSubclasses { get; init; } = true;

    /// <summary>Limits attempts at creating unspecific objects.</summary>
    [ConfigurableOption]
    public Limiter ObjectCreateAttempts { get; init; } = Limiter.Few;

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

    /// <summary><inheritdoc cref="DesignDefaults.IncludeInfinityAndNaNGeneration"/></summary>
    [ConfigurableOption]
    public bool IncludeInfinityAndNaNGeneration { get; init; } =
        DesignDefaults.IncludeInfinityAndNaNGeneration;

    /// <summary>
    ///     If object contents must also be randomized for randomization to be successful.
    /// </summary>
    /// <remarks>
    ///     Generally disabled when creating the specific type
    ///     provides randomization (such as picking a subclass).
    /// </remarks>
    [ConfigurableOption]
    public bool ContentRandomizationRequired { get; init; } = true;

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
            ContentRandomizationRequired = Config.GetValue(section, ContentRandomizationRequired),
            RandomizerCreateAttempts = Config.GetValue(section, RandomizerCreateAttempts),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            PreferLocalSubclasses = Config.GetValue(section, PreferLocalSubclasses),
            ObjectCreateAttempts = Config.GetValue(section, ObjectCreateAttempts),
            CollectionAttempts = Config.GetValue(section, CollectionAttempts),
            StringCharacterSet = Config.GetChars(section, StringCharacterSet),
            CollectionMinSize = Config.GetValue(section, CollectionMinSize),
            CollectionMaxSize = Config.GetValue(section, CollectionMaxSize),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxHintRecursion = Config.GetValue(section, MaxHintRecursion),
            StringMinSize = Config.GetValue(section, StringMinSize),
            StringMaxSize = Config.GetValue(section, StringMaxSize),
            IncludeInfinityAndNaNGeneration = Config.GetValue(
                section,
                IncludeInfinityAndNaNGeneration
            ),
        };
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
        ArgumentGuard.ThrowIfNull(Gen);

        return Math.Max(0, (setMax < setMin) ? Gen.Next(setMax, setMin) : Gen.Next(setMin, setMax));
    }
}
