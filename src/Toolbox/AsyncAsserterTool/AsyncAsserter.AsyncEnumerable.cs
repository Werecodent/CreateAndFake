using System.Text;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool.Categories;
using CreateAndFake.Design.Content;

namespace CreateAndFake.AsyncAsserterTool;

/// <inheritdoc cref="IAsyncAsserter"/>
public partial class AsyncAsserter : IAsyncEnumerableAsserter
{
    /// <inheritdoc/>
    public virtual Task FailAsync<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        return FailAsync(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        localOptions.Asserter.Fail(
            await AsyncEnumHelper.ToListAsync(collection).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task IsEmptyAsync<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        return IsEmptyAsync(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return HasCountAsync(0, collection, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task IsNotEmptyAsync<T>(IAsyncEnumerable<T>? collection, string? details = null)
    {
        return IsNotEmptyAsync(collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                "Expected collection with elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        await foreach (T item in collection.ConfigureAwait(false))
        {
            throw new AssertException(
                "Expected collection with elements, but was empty.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        string? details = null
    )
    {
        return HasCountAsync(count, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsyncAsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        localOptions.Asserter.HasCount(
            count,
            await AsyncEnumHelper.ToListAsync(collection).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task ContainsAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        string? details
    )
    {
        return ContainsAsync(content, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsAsync<T>(
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

            _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine();
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
    public virtual Task ContainsNotAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        string? details = null
    )
    {
        return ContainsNotAsync(content, collection, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsNotAsync<T>(
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

            _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine();
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
