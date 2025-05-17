using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsyncEnumerableAsserter
{
    private static async Task<IEnumerable<T>?> Collect<T>(
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration
    )
    {
        if (collection == null)
        {
            return null;
        }

        List<T> results = [];
        await foreach (T item in collection.ConfigureAwait(false))
        {
            results.Add(item);
        }
        return results;
    }

    /// <inheritdoc/>
    public async Task Is<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    )
    {
        Is(
            await Collect(expected, Unconfigured).ConfigureAwait(false),
            await Collect(actual, Unconfigured).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public async Task Is<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Is(
            await Collect(expected, optionConfiguration).ConfigureAwait(false),
            await Collect(actual, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public async Task IsNot<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    )
    {
        IsNot(
            await Collect(expected, Unconfigured).ConfigureAwait(false),
            await Collect(actual, Unconfigured).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public async Task IsNot<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        IsNot(
            await Collect(expected, optionConfiguration).ConfigureAwait(false),
            await Collect(actual, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task ValuesEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    )
    {
        ValuesEqual(
            await Collect(expected, Unconfigured).ConfigureAwait(false),
            await Collect(actual, Unconfigured).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task ValuesEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ValuesEqual(
            await Collect(expected, optionConfiguration).ConfigureAwait(false),
            await Collect(actual, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task ValuesNotEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    )
    {
        ValuesNotEqual(
            await Collect(expected, Unconfigured).ConfigureAwait(false),
            await Collect(actual, Unconfigured).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task ValuesNotEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ValuesNotEqual(
            await Collect(expected, optionConfiguration).ConfigureAwait(false),
            await Collect(actual, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task AreUnique<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    )
    {
        AreUnique(
            await Collect(expected, Unconfigured).ConfigureAwait(false),
            await Collect(actual, Unconfigured).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task AreUnique<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AreUnique(
            await Collect(expected, optionConfiguration).ConfigureAwait(false),
            await Collect(actual, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public virtual async Task Fail<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        Fail(await Collect(collection, Unconfigured).ConfigureAwait(false), details);
    }

    /// <inheritdoc/>
    public virtual async Task Fail<T>(
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Fail(
            await Collect(collection, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task IsEmpty<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        IsEmpty(await Collect(collection, Unconfigured).ConfigureAwait(false), details);
    }

    /// <inheritdoc/>
    public virtual async Task IsEmpty<T>(
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        IsEmpty(
            await Collect(collection, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task IsNotEmpty<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        IsNotEmpty(await Collect(collection, Unconfigured).ConfigureAwait(false), details);
    }

    /// <inheritdoc/>
    public virtual async Task IsNotEmpty<T>(
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        IsNotEmpty(
            await Collect(collection, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task HasCount<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        string? details = null
    )
    {
        HasCount(count, await Collect(collection, Unconfigured).ConfigureAwait(false), details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCount<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HasCount(
            count,
            await Collect(collection, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task Contains<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        string? details
    )
    {
        Contains(content, await Collect(collection, Unconfigured).ConfigureAwait(false), details);
    }

    /// <inheritdoc/>
    public virtual async Task Contains<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details
    )
    {
        Contains(
            content,
            await Collect(collection, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task ContainsNot<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        string? details = null
    )
    {
        ContainsNot(
            content,
            await Collect(collection, Unconfigured).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual async Task ContainsNot<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ContainsNot(
            content,
            await Collect(collection, optionConfiguration).ConfigureAwait(false),
            optionConfiguration,
            details
        );
    }
}
