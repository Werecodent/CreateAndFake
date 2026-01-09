using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Hints;

namespace CreateAndFake.ValuerTool;

/// <inheritdoc cref="IValuer"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Valuer(ValuerOptions options) : IValuer
{
    /// <summary>Default set of hints to use for comparisons.</summary>
    internal static readonly ImmutableArray<CompareHint> DefaultHints =
    [
        new TaskCompareHint(),
        new AsyncEnumerableCompareHint(),
        new EarlyFailCompareHint(),
        new FallbackCompareHint(),
        new FakedCompareHint(),
        new ValueEquatableCompareHint(),
        new ValuerAsyncComparableCompareHint(),
        new ValuerComparableCompareHint(),
        new ValuerEquatableCompareHint(),
        new EquatableCompareHint(),
        new StringDictionaryCompareHint(),
        new DictionaryCompareHint(),
        new EnumerableCompareHint(),
        new SeededRandomCompareHint(),
        new MemberInfoCompareHint(),
        new MethodBaseCompareHint(),
        new ParameterInfoCompareHint(),
        new ObjectCompareHint(BindingFlags.Public | BindingFlags.Instance),
        new ObjectCompareHint(BindingFlags.NonPublic | BindingFlags.Instance),
        new StatelessCompareHint(),
    ];

    /// <inheritdoc/>
    public ValuerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Generators used to copy specific types.</summary>
    private readonly ImmutableArray<CompareHint> _hints = BuildHints(options);

    /// <summary>Builds hints to use for randomization based upon <paramref name="newOptions"/>.</summary>
    /// <param name="newOptions">Configuration for randomization.</param>
    /// <returns>Built hints to use.</returns>
    private static ImmutableArray<CompareHint> BuildHints(ValuerOptions newOptions)
    {
        return newOptions.IncludeDefaultHints
            ? newOptions.Hints.AddRange(DefaultHints)
            : newOptions.Hints;
    }

    /// <summary>Picks hints to use for randomization based upon <paramref name="localOptions"/>.</summary>
    /// <param name="localOptions">Potentially modified configuration to use.</param>
    /// <returns>Cached hints if possible; built hints otherwise.</returns>
    private ImmutableArray<CompareHint> SelectHints(ValuerOptions localOptions)
    {
        return
            Options.IncludeDefaultHints == localOptions.IncludeDefaultHints
            && Options.Hints == localOptions.Hints
            ? _hints
            : BuildHints(localOptions);
    }

    private ValuerChainer CreateChainer(ValuerMod? optionConfiguration)
    {
        ValuerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        return new ValuerChainer(localOptions, new ValuerEngine(SelectHints(localOptions)));
    }

    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        string? typeName = (expected ?? actual)?.GetType().Name;
        try
        {
            return [.. CreateChainer(optionConfiguration).Compare(expected, actual)];
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue comparing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        string? typeName = (expected ?? actual)?.GetType().Name;
        try
        {
            return CreateChainer(optionConfiguration).CompareAsync(expected, actual);
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue comparing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item)
    {
        return GetHashCode(item, null);
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, ValuerMod? optionConfiguration)
    {
        string? typeName = item?.GetType().Name;
        try
        {
            return CreateChainer(optionConfiguration).GetHashCode(item);
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue hashing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public async Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        string? typeName = item?.GetType().Name;
        try
        {
            return await CreateChainer(optionConfiguration)
                .GetHashCodeAsync(item, canceler)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new ToolException($"Issue hashing type '{typeName}'.", e);
        }
    }

    /// <inheritdoc/>
    public new bool Equals(object? x, object? y)
    {
        return !Compare(x, y).Any();
    }

    /// <inheritdoc/>
    public bool Equals(object? x, object? y, ValuerMod? optionConfiguration)
    {
        return !Compare(x, y, optionConfiguration).Any();
    }

    /// <inheritdoc/>
    public async Task<bool> EqualsAsync(
        object? x,
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return !await AsyncEnumHelper
            .HasAnyAsync(CompareAsync(x, y, optionConfiguration), canceler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IValuer WithOptions(ValuerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new Valuer(optionConfiguration.Invoke(Options));
    }
}
