using System.Diagnostics.CodeAnalysis;
using System.Text;
using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterAsyncEnumerable
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
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual async Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        Fail(
            await AsyncSeriesHelper
                .ToListAsync(collection, localOptions.Valuer.Options.IterationLimit, canceler)
                .ConfigureAwait(false),
            optionConfiguration != null ? _ => localOptions : null,
            details
        );
    }

    /// <inheritdoc/>
    public Task DebugAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return DebugAsync(collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public async Task DebugAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string? content = AsString(
            await AsyncSeriesHelper
                .ToListAsync(collection, localOptions.Valuer.Options.IterationLimit, canceler)
                .ConfigureAwait(false),
            localOptions
        );

        if (localOptions.DebugAssertsFail)
        {
            throw new AssertException(
                $"{nameof(AsserterOptions.DebugAssertsFail)} set to '{true}'.",
                details,
                localOptions.Gen.InitialSeed,
                content
            );
        }
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
        canceler.ThrowIfCancellationRequested();

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
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (collection == null)
        {
            throw new AssertException(
                $"Expected collection of '{count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        await AsyncSeriesHelper
            .ForEachAsync(
                collection,
                localOptions.Valuer.Options.IterationLimit,
                canceler,
                item => _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine()
            )
            .ConfigureAwait(false);

        if (i != count)
        {
            throw new AssertException(
                $"Expected collection of '{count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task HasCountLessThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasCountLessThanAsync(count, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCountLessThanAsync<T>(
        int count,
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
                $"Expected collection of '< {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        await AsyncSeriesHelper
            .ForEachAsync(
                collection,
                localOptions.Valuer.Options.IterationLimit,
                canceler,
                item => _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine()
            )
            .ConfigureAwait(false);

        if (i >= count)
        {
            throw new AssertException(
                $"Expected collection of '< {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task HasCountLessOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasCountLessOrExactlyAsync(count, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCountLessOrExactlyAsync<T>(
        int count,
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
                $"Expected collection of '<= {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        await AsyncSeriesHelper
            .ForEachAsync(
                collection,
                localOptions.Valuer.Options.IterationLimit,
                canceler,
                item => _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine()
            )
            .ConfigureAwait(false);

        if (i > count)
        {
            throw new AssertException(
                $"Expected collection of '<= {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task HasCountMoreThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasCountMoreThanAsync(count, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCountMoreThanAsync<T>(
        int count,
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
                $"Expected collection of '> {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        await AsyncSeriesHelper
            .ForEachAsync(
                collection,
                localOptions.Valuer.Options.IterationLimit,
                canceler,
                item => _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine()
            )
            .ConfigureAwait(false);

        if (i <= count)
        {
            throw new AssertException(
                $"Expected collection of '> {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task HasCountMoreOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasCountMoreOrExactlyAsync(count, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task HasCountMoreOrExactlyAsync<T>(
        int count,
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
                $"Expected collection of '>= {count}' elements, but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        StringBuilder contents = new();

        int i = 0;
        await AsyncSeriesHelper
            .ForEachAsync(
                collection,
                localOptions.Valuer.Options.IterationLimit,
                canceler,
                item => _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine()
            )
            .ConfigureAwait(false);

        if (i < count)
        {
            throw new AssertException(
                $"Expected collection of '>= {count}' elements, but was '{i}'.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public virtual Task ContainsAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ContainsAsync(content, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsAsync<T>(
        T? content,
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
                $"Expected collection to contain '{content}', but was 'null'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }

        int i = 0;
        bool found = false;
        StringBuilder contents = new();
        await AsyncSeriesHelper
            .ForEachAsync(
                collection,
                localOptions.Valuer.Options.IterationLimit,
                canceler,
                async item =>
                {
                    found |= await localOptions
                        .Valuer.EqualsAsync(content, item, canceler)
                        .ConfigureAwait(false);

                    _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine();
                }
            )
            .ConfigureAwait(false);

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
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ContainsNotAsync(content, collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ContainsNotAsync<T>(
        T? content,
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
        await AsyncSeriesHelper
            .ForEachAsync(
                collection,
                localOptions.Valuer.Options.IterationLimit,
                canceler,
                async item =>
                {
                    notFound &= !await localOptions
                        .Valuer.EqualsAsync(content, item, canceler)
                        .ConfigureAwait(false);

                    _ = contents.Append('[').Append(i++).Append("]:").Append(item).AppendLine();
                }
            )
            .ConfigureAwait(false);

        if (!notFound)
        {
            throw new AssertException(
                $"Expected collection to not contain '{content}' but did.",
                details,
                localOptions.Gen.InitialSeed,
                contents.ToString()
            );
        }
    }

    /// <inheritdoc/>
    public Task<TException> ThrowsAsync<TException, TContent>(
        IAsyncEnumerable<TContent>? collection,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return ThrowsAsync<TException, TContent>(collection, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public async Task<TException> ThrowsAsync<TException, TContent>(
        IAsyncEnumerable<TContent>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(collection);

        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage =
            $"Expected exception of type '{GenericConverter.ExpandName<TException>()}' but received: ";
        try
        {
            await foreach (
                TContent item in collection.WithCancellation(canceler).ConfigureAwait(false)
            )
            {
                await Disposer.CleanupAsync(item).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            return UnwrapException<TException>(e, errorMessage, localOptions, details);
        }

        throw new AssertException(errorMessage + "None", details, localOptions.Gen.InitialSeed);
    }

    /// <inheritdoc/>
    public Task<Exception> ThrowsExceptionAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsAsync<Exception, T>(collection, canceler, details);
    }

    /// <inheritdoc/>
    public Task<Exception> ThrowsExceptionAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsAsync<Exception, T>(collection, canceler, optionConfiguration, details);
    }
}
