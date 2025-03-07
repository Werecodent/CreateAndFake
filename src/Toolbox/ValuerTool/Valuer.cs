using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.ValuerTool.CompareHints;

namespace CreateAndFake.ValuerTool;

/// <inheritdoc cref="IValuer"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public sealed class Valuer(ValuerOptions options) : IValuer
{
    /// <summary>Default set of hints to use for comparisons.</summary>
    private static readonly ImmutableArray<CompareHint> _DefaultHints =
    [
        new EarlyFailCompareHint(),
        new FallbackCompareHint(),
        new FakedCompareHint(),
        new TaskCompareHint(),
        new ValueEquatableCompareHint(),
        new ValuerEquatableCompareHint(),
        new EquatableCompareHint(),
        new AsyncEnumerableCompareHint(),
        new StringDictionaryCompareHint(),
        new DictionaryCompareHint(),
        new EnumerableCompareHint(),
        new SeededRandomCompareHint(),
        new MemberInfoCompareHint(),
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
            ? newOptions.Hints.AddRange(_DefaultHints)
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
    public int GetHashCode(object? item)
    {
        return GetHashCode(item, (ValuerMod?)null);
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, ValuerMod? optionConfiguration = null)
    {
        ValuerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        string? typeName = item?.GetType().Name;
        try
        {
            return GetHashCode(item, new ValuerChainer(localOptions, this, GetHashCode, Compare));
        }
        catch (InsufficientExecutionStackException e)
        {
            throw new InsufficientExecutionStackException(
                $"Ran into infinite generation trying to hash type '{typeName}'.",
                e
            );
        }
    }

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="GetHashCode(object)"/>
    private int GetHashCode(object? item, ValuerChainer chainer)
    {
        HashCodeHintResult? result = SelectHints(chainer.Options)
            .Select(h => h.TryGetHashCode(item, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{item?.GetType().FullName}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }

    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        ValuerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        string? typeName = (expected ?? actual)?.GetType().Name;
        try
        {
            return Compare(
                expected,
                actual,
                new ValuerChainer(localOptions, this, GetHashCode, Compare)
            );
        }
        catch (InsufficientExecutionStackException e)
        {
            throw new InsufficientExecutionStackException(
                $"Ran into infinite generation trying to compare type '{typeName}'.",
                e
            );
        }
    }

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="Compare(object,object,ValuerMod)"/>
    private IEnumerable<Difference> Compare(object? expected, object? actual, ValuerChainer chainer)
    {
        if (ReferenceEquals(expected, actual))
        {
            return [];
        }

        DifferenceHintResult? result = SelectHints(chainer.Options)
            .Select(h => h.TryCompare(expected, actual, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{expected?.GetType().FullName}' not supported by the valuer. "
                    + "Create a hint to generate the type and pass it to the valuer."
            );
        }
    }
}
