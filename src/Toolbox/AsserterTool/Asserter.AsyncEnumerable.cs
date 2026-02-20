using System.Text;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Design.Content;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsyncEnumerableAsserter
{
    /// <inheritdoc/>
    public virtual Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return FailAsync(collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Fail(
            await AsyncEnumHelper.ToListAsync(collection, canceler).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return IsEmptyAsync(collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return HasCountAsync(0, collection, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return IsNotEmptyAsync(collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                "Expected collection with elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        bool hasItems = false;
        await foreach (T item in collection.WithCancellation(canceler).ConfigureAwait(false))
        {
            hasItems = true;
            break;
        }

        if (!hasItems)
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
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasCountAsync(count, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        HasCount(
            count,
            await AsyncEnumHelper.ToListAsync(collection, canceler).ConfigureAwait(false),
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task ContainsAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details
    )
    {
        return ContainsAsync(content, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
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
        await foreach (T item in collection.WithCancellation(canceler).ConfigureAwait(false))
        {
            found =
                found
                || await localOptions
                    .Valuer.EqualsAsync(content, item, canceler)
                    .ConfigureAwait(false);

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
        CancellationToken canceler,
        string? details = null
    )
    {
        return ContainsNotAsync(content, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsNotAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            return;
        }

        int i = 0;
        bool notFound = true;
        StringBuilder contents = new();
        await foreach (T item in collection.WithCancellation(canceler).ConfigureAwait(false))
        {
            notFound &= !await localOptions
                .Valuer.EqualsAsync(content, item, canceler)
                .ConfigureAwait(false);

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
