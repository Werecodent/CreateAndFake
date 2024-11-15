using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.AsserterTool.Categories;

namespace CreateAndFake.Toolbox.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IDelegateAsserter
{
    /// <inheritdoc/>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Rethrows all at end.")]
    public virtual void CheckAll(params IEnumerable<Action> cases)
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

        if (errors.Any(e => e != null))
        {
            throw new AggregateException(
                "Cases failed: " + string.Join(", ", Enumerable.Range(0, errors.Count).Where(i => errors[i] != null)) + " -",
                errors.Where(e => e != null).Select(e => e!));
        }
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Action? behavior, string? details = null) where T : Exception
    {
        return Throws<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Action? behavior, AsserterMod optionConfiguration, string? details = null) where T : Exception
    {
        return Throws<T>((Delegate?)behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Func<object?>? behavior, string? details = null) where T : Exception
    {
        return Throws<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Func<object?>? behavior, AsserterMod optionConfiguration, string? details = null) where T : Exception
    {
        return Throws<T>((Delegate?)behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Delegate? behavior, string? details = null) where T : Exception
    {
        return Throws<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual T Throws<T>(Delegate? behavior, AsserterMod optionConfiguration, string? details = null) where T : Exception
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);

        string errorMessage = $"Expected exception of type '{typeof(T).FullName}'.";
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
        catch (T e)
        {
            return e;
        }
        catch (AggregateException e)
        {
            if (e.InnerExceptions.Count == 1 && e.InnerExceptions[0] is T actual)
            {
                return actual;
            }
            else
            {
                throw new AssertException(errorMessage, details, localOptions.Gen.InitialSeed, e);
            }
        }
        catch (Exception e)
        {
            throw new AssertException(errorMessage, details, localOptions.Gen.InitialSeed, e);
        }

        throw new AssertException(errorMessage, details, localOptions.Gen.InitialSeed);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Action? behavior, string? details) where T : Exception
    {
        ThrowsNo<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Action? behavior, AsserterMod optionConfiguration, string? details) where T : Exception
    {
        ThrowsNo<T>((Delegate?)behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Func<object?>? behavior, string? details) where T : Exception
    {
        ThrowsNo<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Func<object?>? behavior, AsserterMod optionConfiguration, string? details) where T : Exception
    {
        ThrowsNo<T>((Delegate?)behavior, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Delegate? behavior, string? details) where T : Exception
    {
        ThrowsNo<T>(behavior, Unconfigured, details);
    }

    /// <inheritdoc/>
    public virtual void ThrowsNo<T>(Delegate? behavior, AsserterMod optionConfiguration, string? details) where T : Exception
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
                Disposer.Cleanup(((dynamic?)behavior)?.Invoke());
            }
        }
        catch (Exception e)
        {
            if (e is T)
            {
                throw new AssertException("Expected no exception.", details, localOptions.Gen.InitialSeed, e);
            }
        }
    }
}
