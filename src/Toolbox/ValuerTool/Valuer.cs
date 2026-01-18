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

    /// <summary>Handles hint based comparisons.</summary>
    private static readonly ValuerEngine _engine = new(DefaultHints);

    /// <inheritdoc/>
    public ValuerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _engine.SupportedTypes;

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
            return
            [
                .. new ValuerChainer(Options, _engine).Compare(
                    expected,
                    actual,
                    optionConfiguration
                ),
            ];
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
            return new ValuerChainer(Options, _engine).CompareAsync(
                expected,
                actual,
                optionConfiguration
            );
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
            return new ValuerChainer(Options, _engine).GetHashCode(item, optionConfiguration);
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
            return await new ValuerChainer(Options, _engine)
                .GetHashCodeAsync(item, canceler, optionConfiguration)
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
