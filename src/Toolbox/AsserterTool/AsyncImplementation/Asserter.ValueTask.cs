using CreateAndFake.AsserterTool.AsyncCategories;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterValueTask
{
    /// <inheritdoc/>
    public virtual Task<T> HasResultAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasResultAsync(operation, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> HasResultAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        IsNotNull(operation, optionConfiguration, details);
        return await operation.Value.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(operation, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> ThrowsAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage =
            $"Expected exception of type '{GenericConverter.ExpandName<T>()}' but received: ";
        if (operation != null)
        {
            try
            {
                await operation.Value.ConfigureAwait(false);
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
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return ThrowsAsync<TException, TContent>(operation, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage =
            $"Expected exception of type '{GenericConverter.ExpandName<TException>()}' but received: ";
        if (operation != null)
        {
            try
            {
                await Disposer
                    .CleanupAsync(await operation.Value.ConfigureAwait(false))
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
    public Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsExceptionAsync(operation, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsAsync<Exception, T>(operation, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(operation, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        canceler.ThrowIfCancellationRequested();

        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (operation != null)
        {
            try
            {
                await operation.Value.ConfigureAwait(false);
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
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return ThrowsNoAsync<TException, TContent>(operation, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        canceler.ThrowIfCancellationRequested();

        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (operation != null)
        {
            try
            {
                await Disposer
                    .CleanupAsync(await operation.Value.ConfigureAwait(false))
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
    public virtual Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsNoExceptionAsync(operation, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsNoAsync<Exception, T>(operation, canceler, optionConfiguration, details);
    }
}
