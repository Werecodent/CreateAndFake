using Werecodent.CreateAndFake.AsserterTool.Categories;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterDelegate
{
    /// <inheritdoc/>
    public virtual T Throws<T>(Delegate? behavior, string? details = null)
        where T : Exception
    {
        return Throws<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        VerifyCanCall(behavior, localOptions, details);

        string errorMessage =
            $"Expected exception of type '{GenericConverter.ExpandName<T>()}' but received: ";
        try
        {
            JustInvoke(behavior);
        }
        catch (Exception e)
        {
            return UnwrapException<T>(e, errorMessage, localOptions, details);
        }

        throw new AssertException(errorMessage + "None", details, localOptions.Gen.InitialSeed);
    }

    /// <inheritdoc/>
    public virtual Exception ThrowsException(Delegate? behavior, string? details = null)
    {
        return ThrowsException(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual Exception ThrowsException(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return Throws<Exception>(behavior, optionConfiguration, details);
    }

    private static T UnwrapException<T>(
        Exception e,
        string errorMessage,
        AsserterOptions localOptions,
        string? details
    )
        where T : Exception
    {
        if (localOptions.DisableAssertThrowCatching)
        {
            throw e;
        }

        if (e is T noWrap)
        {
            return noWrap;
        }

        Exception error =
            (e is AggregateException agg && agg.InnerExceptions.Count == 1)
                ? agg.InnerExceptions[0]
                : e;

        return error as T
            ?? throw new AssertException(
                errorMessage + GenericConverter.ExpandName(e),
                details,
                localOptions.Gen.InitialSeed,
                e
            );
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Delegate? behavior, string? details = null)
        where T : Exception
    {
        ThrowsNo<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        VerifyCanCall(behavior, localOptions, details);
        try
        {
            JustInvoke(behavior);
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

    /// <inheritdoc/>
    public virtual void ThrowsNoException(Delegate? behavior, string? details = null)
    {
        ThrowsNoException(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNoException(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ThrowsNo<Exception>(behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual T HasResult<T>(Delegate? behavior, string? details = null)
    {
        return HasResult<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T HasResult<T>(
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        VerifyCanCall(behavior, localOptions, details);

        object? result;
        try
        {
            result = Invoke(behavior);
        }
        catch (Exception e)
        {
            throw new AssertException(
                "Expected no exception.",
                details,
                localOptions.Gen.InitialSeed,
                e
            );
        }

        if (result is T data)
        {
            return data;
        }
        else
        {
            throw new AssertException(
                $"Expected result type of '{GenericConverter.ExpandName<T>()},"
                    + $" but was '{GenericConverter.ExpandName(result)}'.",
                details,
                localOptions.Gen.InitialSeed
            );
        }
    }

    /// <inheritdoc/>
    public virtual T HasResult<T>(T content, Delegate? behavior, string? details = null)
    {
        return HasResult(content, behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T HasResult<T>(
        T content,
        Delegate? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        T result = HasResult<T>(behavior, _ => localOptions, details);
        Is(content, result, _ => localOptions, details);
        return result;
    }

    /// <inheritdoc/>
    public virtual Task<T> HasResultAsync<T>(
        T content,
        Delegate? behavior,
        CancellationToken canceler,
        string? details = null
    )
    {
        return HasResultAsync(content, behavior, canceler, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual async Task<T> HasResultAsync<T>(
        T content,
        Delegate? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        T result = HasResult<T>(behavior, _ => localOptions, details);
        await IsAsync(content, result, canceler, _ => localOptions, details).ConfigureAwait(false);
        return result;
    }

    private static void VerifyCanCall(Delegate? behavior, AsserterOptions options, string? details)
    {
        if (behavior is null)
        {
            return;
        }

        if (behavior.Method.GetParameters().Length != 0)
        {
            throw new AssertException(
                "Delegate to test must not require an instance or arguments.",
                details,
                options.Gen.InitialSeed
            );
        }
    }

    private static void JustInvoke(Delegate? behavior)
    {
        Disposer.Cleanup(Invoke(behavior));
    }

    private static object? Invoke(Delegate? behavior)
    {
        if (behavior == null)
        {
            return null;
        }
        else if (behavior is Action action)
        {
            action.Invoke();
            return VoidType.Instance;
        }
        else if (behavior.GetType().Inherits(typeof(Func<>)))
        {
            return ((dynamic)behavior).Invoke();
        }
        else
        {
            return behavior.DynamicInvoke([]);
        }
    }
}
