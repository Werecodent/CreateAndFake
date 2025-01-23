using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.FakerTool;

/// <inheritdoc/>
/// <typeparam name="T">Return type.</typeparam>
public sealed class Behavior<T> : Behavior
{
    /// <inheritdoc/>>
    public Behavior(Delegate implementation, Times? times = null) : base(implementation, times, 0) { }

    /// <inheritdoc/>
    private Behavior(Delegate implementation, Times times, int calls) : base(implementation, times, calls) { }

    /// <inheritdoc/>
    public override IDeepCloneable DeepClone()
    {
        return new Behavior<T>(Implementation, Limit, Calls);
    }

    /// <summary>Specifies exception behavior for a fake.</summary>
    /// <param name="times">Behavior call limit.</param>
    public static new Behavior<T> Error(Times? times = null)
    {
        return Throw<NotImplementedException>(times);
    }

    /// <inheritdoc cref="Throw{T}(T,Times)"/>
    public static new Behavior<T> Throw<TException>(Times? times = null) where TException : Exception, new()
    {
        return Throw(new TException(), times);
    }

    /// <summary>Specifies a specific exception behavior for a fake.</summary>
    /// <typeparam name="TException">Exception type to throw if called.</typeparam>
    /// <param name="exception">Exception to throw.</param>
    /// <param name="times">Behavior call limit.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static new Behavior<T> Throw<TException>(TException exception, Times? times = null) where TException : Exception
    {
        return Set<T>(() => throw exception, times);
    }
}
