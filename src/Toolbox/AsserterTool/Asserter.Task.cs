using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Design.Content;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : ITaskAsserter
{
    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(Task? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(
        Task? behavior,
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
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(Task<object?>? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, Unconfigured, details);
    }

#pragma warning disable CA1031 // Rethrows.

    /// <inheritdoc/>
    public virtual async Task<T> ThrowsAsync<T>(
        Task<object?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage = $"Expected exception of type '{typeof(T).FullName}' but received: ";
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

#pragma warning restore CA1031

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(Func<Task?>? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(
        Func<Task?>? behavior,
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
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task<T> ThrowsAsync<T>(Func<Task<object?>?>? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsAsync<T>(behavior, Unconfigured, details);
    }

#pragma warning disable CA1031 // Rethrows.

    /// <inheritdoc/>
    public virtual async Task<T> ThrowsAsync<T>(
        Func<Task<object?>?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage = $"Expected exception of type '{typeof(T).FullName}' but received: ";

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

#pragma warning restore CA1031

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(Task? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        Task? behavior,
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
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(Task<object?>? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<T>(
        Task<object?>? behavior,
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
    public virtual Task ThrowsNoAsync<T>(Func<Task?>? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(
        Func<Task?>? behavior,
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
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc/>
    public virtual Task ThrowsNoAsync<T>(Func<Task<object?>?>? behavior, string? details = null)
        where T : Exception
    {
        return ThrowsNoAsync<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task ThrowsNoAsync<T>(
        Func<Task<object?>?>? behavior,
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
