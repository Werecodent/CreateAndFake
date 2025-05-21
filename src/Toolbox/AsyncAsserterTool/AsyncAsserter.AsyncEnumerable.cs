using System.Text;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool.Categories;

namespace CreateAndFake.AsyncAsserterTool;

/// <inheritdoc cref="IAsyncAsserter"/>
public partial class AsyncAsserter : IAsyncEnumerableAsserter
{
    private static async Task<IEnumerable<T>?> Collect<T>(IAsyncEnumerable<T>? collection)
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
    public virtual Task Fail<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        return Fail(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task Fail<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        localOptions.Asserter.Fail(await Collect(collection).ConfigureAwait(false), details);
    }

    /// <inheritdoc/>
    public virtual Task IsEmpty<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        return IsEmpty(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task IsEmpty<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return HasCount(0, collection, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task IsNotEmpty<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        return IsNotEmpty(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task IsNotEmpty<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection with elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
        else if (!await collection.GetAsyncEnumerator().MoveNextAsync().ConfigureAwait(false))
        {
            throw new AssertException(
                "Expected collection with elements, but was empty.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task HasCount<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        string? details = null
    )
    {
        return HasCount(count, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCount<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        localOptions.Asserter.HasCount(
            count,
            await Collect(collection).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task Contains<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        string? details
    )
    {
        return Contains(content, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task Contains<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}', but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        int i = 0;
        bool found = false;
        StringBuilder contents = new();
        await foreach (T item in collection.ConfigureAwait(false))
        {
            found =
                found || await localOptions.Valuer.EqualsAsync(content, item).ConfigureAwait(false);

            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (!found)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task ContainsNot<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        string? details = null
    )
    {
        return ContainsNot(content, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsNot<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            return;
        }

        int i = 0;
        bool notFound = true;
        StringBuilder contents = new();
        await foreach (T item in collection.ConfigureAwait(false))
        {
            notFound &= !await localOptions.Valuer.EqualsAsync(content, item).ConfigureAwait(false);

            _ = contents.Append('[').Append(i).Append("]:").Append(item).AppendLine();
        }

        if (!notFound)
        {
            throw new AssertException(
                $"Expected collection to contain '{content}' but didn't.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }
}
