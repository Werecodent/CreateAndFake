using System.Collections.Frozen;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.RandomizerTool;

/// <summary>Configuration for manipulating <see cref="IRandomizer"/> behavior.</summary>
public sealed record RandomizerOptions : ToolHintOptions<RandomizerOptions, ICreateHint>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Randomizer);

    /// <inheritdoc cref="IValuer"/>
    public required IValuer Valuer { get; init; }

    /// <inheritdoc cref="IFaker"/>
    public required IFaker Faker { get; init; }

    /// <summary>
    ///     When generating random objects, subclasses from the originating assembly are preferred
    ///     if  <see langword="true"/>. Otherwise, subclasses from all loaded assemblies are used.
    /// </summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool PreferLocalSubclasses { get; init; } = true;

    /// <summary>Limits attempts at creating unspecific objects.</summary>
    /// <value><c>Limiter.Few</c></value>
    [ConfigurableOption]
    public Limiter ObjectCreateAttempts { get; init; } = Limiter.Few;

    /// <summary>Limits population attempts for collections when encountering problems.</summary>
    /// <value><c>Limiter.Score</c></value>
    [ConfigurableOption]
    public Limiter CollectionAttempts { get; init; } = Limiter.Score;

    /// <summary>Inclusive minimum size for created collections.</summary>
    /// <value><c>1</c></value>
    [ConfigurableOption]
    public int CollectionMinSize { get; init; } = 1;

    /// <summary>Inclusive maximum size for created collections.</summary>
    /// <value><c>3</c></value>
    [ConfigurableOption]
    public int CollectionMaxSize { get; init; } = 3;

    /// <summary>Inclusive minimum size for created strings.</summary>
    /// <value><c>7</c></value>
    [ConfigurableOption]
    public int StringMinSize { get; init; } = 7;

    /// <summary>Inclusive maximum size for created string.</summary>
    /// <value><c>12</c></value>
    [ConfigurableOption]
    public int StringMaxSize { get; init; } = 12;

    /// <summary>Characters to include in random strings.</summary>
    /// <value><c>[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890]</c></value>
    [ConfigurableOption]
    public FrozenSet<char> StringCharacterSet { get; init; } =
        FrozenSet.ToFrozenSet("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");

    /// <summary>
    ///     If object contents must also be randomized for randomization to be successful.
    /// </summary>
    /// <remarks>
    ///     Generally disabled when creating the specific type
    ///     provides randomization (such as picking a subclass).
    /// </remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool ContentRandomizationRequired { get; init; } = true;

    /// <summary>Limits attempts at matching conditions.</summary>
    /// <value><c>Limiter.Dozen</c></value>
    [ConfigurableOption]
    public Limiter FinalConditionAttemptLimit { get; init; } = Limiter.Dozen;

    /// <summary>Condition for the resulting randomized instance to match.</summary>
    /// <value><see langword="null"/></value>
    public Func<object, bool>? FinalCondition { get; init; } = null;

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal RandomizerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            ContentRandomizationRequired = Config.GetValue(section, ContentRandomizationRequired),
            FinalConditionAttemptLimit = Config.GetValue(section, FinalConditionAttemptLimit),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            PreferLocalSubclasses = Config.GetValue(section, PreferLocalSubclasses),
            ObjectCreateAttempts = Config.GetValue(section, ObjectCreateAttempts),
            CollectionAttempts = Config.GetValue(section, CollectionAttempts),
            StringCharacterSet = Config.GetChars(section, StringCharacterSet),
            CollectionMinSize = Config.GetValue(section, CollectionMinSize),
            CollectionMaxSize = Config.GetValue(section, CollectionMaxSize),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxRecursionDepth = Config.GetValue(section, MaxRecursionDepth),
            StringMinSize = Config.GetValue(section, StringMinSize),
            StringMaxSize = Config.GetValue(section, StringMaxSize),
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
