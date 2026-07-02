using CreateAndFake.AsserterTool.AsyncCategories;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;

namespace CreateAndFake.AsserterTool;

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
    public virtual Task<T> ThrowsAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(
            async () =>
            {
                if (behavior != null)
                {
                    await behavior.ConfigureAwait(false);
                }
                return null;
            },
            canceler,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(
        Task<object?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> ThrowsAsync<T>(
        Task<object?>? behavior,
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
                    .CleanupAsync(await behavior.ConfigureAwait(false))
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
    public virtual Task<T> ThrowsAsync<T>(
        Func<Task?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(
        Func<Task?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(
            async () =>
            {
                Task? task = behavior?.Invoke();
                if (task != null)
                {
                    await task.ConfigureAwait(false);
                }
                return null;
            },
            canceler,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> ThrowsAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage =
            $"Expected exception of type '{GenericConverter.ExpandName<T>()}' but received: ";

        Task<object?>? task = behavior?.Invoke();
        if (task != null)
        {
            try
            {
                await Disposer.CleanupAsync(await task.ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return UnwrapException<T>(e, errorMessage, localOptions, details);
            }
        }

        throw new AssertException(errorMessage + "None", details, localOptions.Gen.InitialSeed);
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
    public virtual Task ThrowsNoAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(
            async () =>
            {
                if (behavior != null)
                {
                    await behavior.ConfigureAwait(false);
                }
                return null;
            },
            canceler,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        Task<object?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<T>(
        Task<object?>? behavior,
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
                    .CleanupAsync(await behavior.ConfigureAwait(false))
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
    public virtual Task ThrowsNoAsync<T>(
        Func<Task?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        Func<Task?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(
            async () =>
            {
                Task? task = behavior?.Invoke();
                if (task != null)
                {
                    await task.ConfigureAwait(false);
                }
                return null;
            },
            canceler,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<T>(
        Func<Task<object?>?>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        Task<object?>? task = behavior?.Invoke();
        if (task != null)
        {
            try
            {
                await Disposer.CleanupAsync(await task.ConfigureAwait(false)).ConfigureAwait(false);
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
}
