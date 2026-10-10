using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterTask
{
    /// <inheritdoc/>
    public virtual Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasResultAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        IsNotNull(behavior, optionConfiguration, details);
        return behavior;
    }

    /// <inheritdoc/>
    public virtual Task<T> HasResultAsync<T>(
        T content,
        Task<T>? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasResultAsync(content, behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> HasResultAsync<T>(
        T content,
        Task<T>? behavior,
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
        Task? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> ThrowsAsync<T>(
        Task? behavior,
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
                await Disposer
                    .CleanupAsync(await Invoker.AwaitAsync(behavior).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return UnwrapException<T>(e, errorMessage, localOptions, details);
            }
        }

        throw new AssertException(errorMessage + "None", details, localOptions.Gen.InitialSeed);
    }

    /// <inheritdoc/>
    public virtual Task<Exception> ThrowsExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsExceptionAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task<Exception> ThrowsExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsAsync<Exception>(behavior, canceler, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (behavior != null)
        {
            try
            {
                await Disposer
                    .CleanupAsync(await Invoker.AwaitAsync(behavior).ConfigureAwait(false))
                    .ConfigureAwait(false);
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
    public virtual Task ThrowsNoExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return ThrowsNoExceptionAsync(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return ThrowsNoAsync<Exception>(behavior, canceler, optionConfiguration, details);
    }
}
