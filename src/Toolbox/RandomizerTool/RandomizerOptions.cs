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

    /// <summary>Flag to prioritize creating subclasses from the originating assembly.</summary>
    /// <remarks>
    ///     All loaded assemblies are prioritized equally if set to <see langword="false"/>. Does not prevent using any
    ///     loaded subclass if there are no subclasses from the originating assembly that can be created successfully.
    /// </remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool PreferLocalSubclasses { get; init; } = true;

    /// <summary>Limits attempts at creating <see cref="object"/>s.</summary>
    /// <remarks>
    ///     Attempts fail upon encountering any issue creating a <see cref="Type"/>, such as not being
    ///     able to randomize an instance's contents when <see cref="RequireRandomizedContents"/> is set.
    /// </remarks>
    /// <value><c>Limiter.Few</c></value>
    [ConfigurableOption]
    public Limiter CreateObjectAttemptLimit { get; init; } = Limiter.Few;

    /// <summary>Limits attempts at creating collections.</summary>
    /// <remarks>Attempts fail generally from failing size requirements due to hash collisions.</remarks>
    /// <value><c>Limiter.Score</c></value>
    [ConfigurableOption]
    public Limiter CreateCollectionAttemptLimit { get; init; } = Limiter.Score;

    /// <summary>Inclusive minimum size for randomized collections.</summary>
    /// <remarks>
    ///     Swaps with <see cref="CollectionMaxSize"/> if greater than it in <see cref="NextCollectionSize"/>.
    /// </remarks>
    /// <value><c>1</c></value>
    [ConfigurableOption]
    public int CollectionMinSize { get; init; } = 1;

    /// <summary>Inclusive maximum size for randomized collections.</summary>
    /// <remarks>
    ///     Swaps with <see cref="CollectionMinSize"/> if less than it in <see cref="NextCollectionSize"/>.
    /// </remarks>
    /// <value><c>3</c></value>
    [ConfigurableOption]
    public int CollectionMaxSize { get; init; } = 3;

    /// <summary>Inclusive minimum size for randomized <see cref="string"/>s.</summary>
    /// <remarks>Swaps with <see cref="StringMaxSize"/> if greater than it in <see cref="NextStringSize"/>.</remarks>
    /// <value><c>7</c></value>
    [ConfigurableOption]
    public int StringMinSize { get; init; } = 7;

    /// <summary>Inclusive maximum size for randomized <see cref="string"/>s.</summary>
    /// <remarks>Swaps with <see cref="StringMinSize"/> if less than it in <see cref="NextStringSize"/>.</remarks>
    /// <value><c>12</c></value>
    [ConfigurableOption]
    public int StringMaxSize { get; init; } = 12;

    /// <summary>Characters to use in randomized <see cref="string"/>s.</summary>
    /// <remarks>Does not affect randomized <see cref="char"/>s.</remarks>
    /// <value><c>[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890]</c></value>
    [ConfigurableOption]
    public FrozenSet<char> StringCharacterSet { get; init; } =
        FrozenSet.ToFrozenSet("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");

    /// <summary>If <see cref="object"/> contents must also be randomized for randomization to be successful.</summary>
    /// <remarks>
    ///     Used to ensure randomized instances have variance with other instances of the same
    ///     <see cref="Type"/> when that <see cref="Type"/> is mutable. Fails if not achieved within
    ///     the limit set by <see cref="CreateObjectAttemptLimit"/>. Automatically disabled when creating
    ///     the specific <see cref="Type"/> provides randomization itself (such as picking a subclass).
    /// </remarks>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool RequireRandomizedContents { get; init; } = true;

    /// <summary>Limits attempts at matching <see cref="FinalCondition"/>.</summary>
    /// <remarks>Only useful when <see cref="FinalCondition"/> is set.</remarks>
    /// <value><c>Limiter.Dozen</c></value>
    [ConfigurableOption]
    public Limiter FinalConditionAttemptLimit { get; init; } = Limiter.Dozen;

    /// <summary>Optional condition that randomized instances must match before being returned.</summary>
    /// <remarks>Fails if not achieved within the limit set by <see cref="FinalConditionAttemptLimit"/>.</remarks>
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
            CreateCollectionAttemptLimit = Config.GetValue(section, CreateCollectionAttemptLimit),
            FinalConditionAttemptLimit = Config.GetValue(section, FinalConditionAttemptLimit),
            RequireRandomizedContents = Config.GetValue(section, RequireRandomizedContents),
            CreateObjectAttemptLimit = Config.GetValue(section, CreateObjectAttemptLimit),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            PreferLocalSubclasses = Config.GetValue(section, PreferLocalSubclasses),
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
