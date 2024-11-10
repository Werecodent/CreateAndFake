using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertBehaviorBase<T>(AsserterOptions options, Delegate? behavior)
    : AssertObjectBase<T>(options, behavior) where T : AssertBehaviorBase<T>
{
    /// <summary>Delegate to check.</summary>
    protected Delegate? Behavior { get; } = behavior;

    /// <summary>Verifies <c>behavior</c> throws an exception.</summary>
    /// <typeparam name="TException">Exception type expected.</typeparam>
    /// <param name="details">Optional failure details to include.</param>
    /// <exception cref="AssertException">If the expected behavior doesn't happen.</exception>
    public virtual TException Throws<TException>(string? details = null) where TException : Exception
    {
        string errorMessage = $"Expected exception of type '{typeof(TException).FullName}'.";
        try
        {
            if (Behavior is Action action)
            {
                action.Invoke();
            }
            else
            {
                Disposer.Cleanup(((dynamic?)Behavior)?.Invoke());
            }
        }
        catch (TException e)
        {
            return e;
        }
        catch (AggregateException e)
        {
            if (e.InnerExceptions.Count == 1 && e.InnerExceptions[0] is TException actual)
            {
                return actual;
            }
            else
            {
                throw new AssertException(errorMessage, details, Options.Gen.InitialSeed, e);
            }
        }
        catch (Exception e)
        {
            throw new AssertException(errorMessage, details, Options.Gen.InitialSeed, e);
        }

        throw new AssertException(errorMessage, details, Options.Gen.InitialSeed);
    }

    /// <summary>Verifies <c>behavior</c> does not throw an exception.</summary>
    /// <param name="details">Optional failure details to include.</param>
    /// <exception cref="AssertException">If the expected behavior doesn't happen.</exception>
    public virtual void ThrowsNoException(string? details = null)
    {
        try
        {
            if (Behavior is Action action)
            {
                action.Invoke();
            }
            else
            {
                Disposer.Cleanup(((dynamic?)Behavior)?.Invoke());
            }
        }
        catch (Exception e)
        {
            throw new AssertException("Expected no exception.", details, Options.Gen.InitialSeed, e);
        }
    }
}
