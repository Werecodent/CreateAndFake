using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IDelegateAsserter
{
    /// <inheritdoc/>
    public virtual void CheckAll(params ICollection<Action> cases)
    {
        if (cases == null)
        {
            return;
        }

        List<Exception?> errors = [];
        foreach (Action test in cases)
        {
            try
            {
                test.Invoke();
                errors.Add(null);
            }
            catch (Exception e)
            {
                errors.Add(e);
            }
        }

        if (errors.Exists(e => e != null))
        {
            throw new AggregateException(
                "Cases failed: "
                    + string.Join(
                        ", ",
                        Enumerable.Range(0, errors.Count).Where(i => errors[i] != null)
                    )
                    + " -",
                errors.Where(e => e != null).Select(e => e!)
            );
        }
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Action? behavior, string? details = null)
        where T : Exception
    {
        return Throws<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        return Throws<T>((Delegate?)behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Func<object?>? behavior, string? details = null)
        where T : Exception
    {
        return Throws<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(
        Func<object?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        return Throws<T>((Delegate?)behavior, optionConfiguration, details);
    }

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

        string errorMessage =
            $"Expected exception of type '{TypeHelper.ExpandedName<T>()}' but received: ";
        try
        {
            if (behavior is Action action)
            {
                action.Invoke();
            }
            else
            {
                Disposer.Cleanup([((dynamic?)behavior)?.Invoke()]);
            }
        }
        catch (Exception e)
        {
            return UnwrapException<T>(e, errorMessage, localOptions, details);
        }

        throw new AssertException(errorMessage + "None", details, localOptions.Gen.InitialSeed);
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
                errorMessage + TypeHelper.ExpandedName(e),
                details,
                localOptions.Gen.InitialSeed,
                e
            );
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Action? behavior, string? details = null)
        where T : Exception
    {
        ThrowsNo<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(
        Action? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        ThrowsNo<T>((Delegate?)behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Func<object?>? behavior, string? details = null)
        where T : Exception
    {
        ThrowsNo<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(
        Func<object?>? behavior,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        ThrowsNo<T>((Delegate?)behavior, optionConfiguration, details);
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
        try
        {
            if (behavior is Action action)
            {
                action.Invoke();
            }
            else
            {
                Disposer.Cleanup([((dynamic?)behavior)?.Invoke()]);
            }
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
