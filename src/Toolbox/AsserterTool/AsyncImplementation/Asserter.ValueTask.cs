using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterValueTask
{
    /// <inheritdoc/>
    public virtual Task<T> HasResultAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasResultAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> HasResultAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        IsNotNull(behavior, optionConfiguration, details);
        return await behavior.Value.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual Task<T> HasResultAsync<T>(
        T content,
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasResultAsync(content, behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> HasResultAsync<T>(
        T content,
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        T result = await HasResultAsync(behavior, canceler, _ => localOptions, details)
            .ConfigureAwait(false);

        await IsAsync(content, result, canceler, _ => localOptions, details).ConfigureAwait(false);
        return result;
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(
        ValueTask? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> ThrowsAsync<T>(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage =
            $"Expected exception of type '{GenericConverter.ExpandName<T>()}' but received: ";
        if (behavior != null)
        {
            try
            {
                await behavior.Value.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return UnwrapException<T>(e, errorMessage, localOptions, details);
            }
        }

        throw new AssertException(errorMessage + "None", details, localOptions.Gen.InitialSeed);
    }

    /// <inheritdoc/>
    public virtual Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return ThrowsAsync<TException, TContent>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage =
            $"Expected exception of type '{GenericConverter.ExpandName<TException>()}' but received: ";
        if (behavior != null)
        {
            try
            {
                await Disposer
                    .CleanupAsync(await behavior.Value.ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return UnwrapException<TException>(e, errorMessage, localOptions, details);
            }
        }

        throw new AssertException(errorMessage + "None", details, localOptions.Gen.InitialSeed);
    }

    /// <inheritdoc/>
    public virtual Task<Exception> ThrowsExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsExceptionAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task<Exception> ThrowsExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsAsync<Exception>(behavior, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsExceptionAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsAsync<Exception, T>(behavior, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        ValueTask? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<T>(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        canceler.ThrowIfCancellationRequested();

        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (behavior != null)
        {
            try
            {
                await behavior.Value.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (e is T)
                {
                    throw new AssertException(
                        $"Expected no exception of type '{typeof(T).Name}'.",
                        details,
                        localOptions.Gen.InitialSeed,
                        e
                    );
                }
            }
        }
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return ThrowsNoAsync<TException, TContent>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        canceler.ThrowIfCancellationRequested();

        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (behavior != null)
        {
            try
            {
                await Disposer
                    .CleanupAsync(await behavior.Value.ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (e is TException)
                {
                    throw new AssertException(
                        $"Expected no exception of type '{typeof(TException).Name}'.",
                        details,
                        localOptions.Gen.InitialSeed,
                        e
                    );
                }
            }
        }
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsNoExceptionAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsNoAsync<Exception>(behavior, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsNoExceptionAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsNoAsync<Exception, T>(behavior, canceler, optionConfiguration, details);
    }
}
