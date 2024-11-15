using System.Reflection;
using System.Runtime.ExceptionServices;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.FakerTool;

/// <summary>Mock behavior for fakes.</summary>
/// <param name="implementation"><inheritdoc cref="Implementation" path="/summary"/></param>
/// <param name="times"><inheritdoc cref="Limit" path="/summary"/></param>
/// <param name="calls">Starting number of calls.</param>
public abstract class Behavior(Delegate implementation, Times? times, int calls) : IDeepCloneable
{
    /// <summary>Set behavior to run.</summary>
    protected Delegate Implementation { get; } = implementation;

    /// <summary>Behavior call limit.</summary>
    protected Times Limit { get; } = times ?? Times.Min(1);

    /// <summary>Times the fake behavior was called.</summary>
    protected internal int Calls { get; private set; } = calls;

    /// <summary>Triggers calling the base method of the given type instead.</summary>
    public Type? BaseCallType { get; private set; } = null;

    /// <inheritdoc/>
    public abstract IDeepCloneable DeepClone();

    /// <summary>Runs the behavior.</summary>
    /// <param name="args">Expected args for the behavior.</param>
    /// <returns>Result of the call.</returns>
    internal object? Invoke(object[] args)
    {
        return Invoke(Implementation, args);
    }

    /// <summary>Runs the behavior.</summary>
    /// <param name="implementation">Delegate to run.</param>
    /// <param name="args">Expected args for the behavior.</param>
    /// <returns>Result of the call.</returns>
    internal object? Invoke(Delegate implementation, object[] args)
    {
        Calls++;
        try
        {
            if (implementation.Method.GetParameters().Length == 0)
            {
                return implementation.DynamicInvoke();
            }
            else
            {
                return implementation.DynamicInvoke(args);
            }
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo? info = (e is TargetInvocationException)
                ? ExceptionDispatchInfo.Capture(e.InnerException!)
                : null;

            info?.Throw();
            throw;
        }
    }

    /// <summary>Checks if call count is in expected range.</summary>
    /// <returns><c>true</c> if in range; <c>false</c> otherwise.</returns>
    internal bool HasExpectedCalls()
    {
        return Limit.IsInRange(Calls);
    }

    /// <returns>Text representation of expected calls.</returns>
    internal string ToExpectedCalls()
    {
        return Limit.ToString();
    }

    /// <summary>Specifies no behavior for a fake.</summary>
    /// <param name="times">Behavior call limit.</param>
    public static Behavior<VoidType> None(Times? times = null)
    {
        return Set(() => { }, times);
    }

    /// <summary>Specifies exception behavior for a fake.</summary>
    /// <param name="times">Behavior call limit.</param>
    public static Behavior<VoidType> Error(Times? times = null)
    {
        return Throw<NotImplementedException>(times);
    }

    /// <summary>Specifies behavior returning null for a fake.</summary>
    /// <typeparam name="T">Null type to return.</typeparam>
    /// <param name="times">Behavior call limit.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static Behavior<T> Null<T>(Times? times = null)
    {
        return Returns(default(T), times);
    }

    /// <summary>Specifies behavior returning the default for a fake.</summary>
    /// <typeparam name="T">Default value type to return.</typeparam>
    /// <param name="times">Behavior call limit.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static Behavior<T> Default<T>(Times? times = null)
    {
        return Returns(default(T), times);
    }

    /// <inheritdoc cref="Base{T,T}"/>
    public static Behavior<VoidType> Base<TBase>(Times? times = null)
    {
        return Base<TBase, VoidType>(times);
    }

    /// <summary>Specifies behavior calling the base implementation.</summary>
    /// <typeparam name="TBase">Type with the base method to call.</typeparam>
    /// <typeparam name="T">Expected value type to return.</typeparam>
    /// <param name="times">Behavior call limit.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static Behavior<T> Base<TBase, T>(Times? times = null)
    {
        Behavior<T> result = Default<T>(times);
        result.BaseCallType = typeof(TBase);
        return result;
    }

    /// <inheritdoc cref="Throw{T}(T,Times)"/>
    public static Behavior<VoidType> Throw<T>(Times? times = null) where T : Exception, new()
    {
        return Throw(new T(), times);
    }

    /// <summary>Specifies a specific exception behavior for a fake.</summary>
    /// <typeparam name="T">Exception type to throw if called.</typeparam>
    /// <param name="exception">Exception to throw.</param>
    /// <param name="times">Behavior call limit.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static Behavior<VoidType> Throw<T>(T exception, Times? times = null) where T : Exception
    {
        return Set(() => throw exception, times);
    }

    /// <summary>Specifies behavior returning a value for a fake.</summary>
    /// <typeparam name="T">Value type to return.</typeparam>
    /// <param name="value">Value to return.</param>
    /// <param name="times">Behavior call limit.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static Behavior<T> Returns<T>(T? value, Times? times = null)
    {
        return Set(() => value, times);
    }

    /// <summary>Specifies behavior returning values for a fake.</summary>
    /// <typeparam name="T">Value type to return.</typeparam>
    /// <param name="values">Sequential values to return.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static Behavior<T> Series<T>(params IEnumerable<T> values)
    {
        IEnumerator<T> gen = (values ?? []).GetEnumerator();
        return Set(() =>
        {
            if (gen.MoveNext())
            {
                return gen.Current;
            }
            else
            {
                return default;
            }
        });
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set(
        Action implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T>(
        Action<T> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2>(
        Action<T1, T2> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3>(
        Action<T1, T2, T3> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4>(
        Action<T1, T2, T3, T4> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5>(
        Action<T1, T2, T3, T4, T5> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6>(
        Action<T1, T2, T3, T4, T5, T6> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7>(
        Action<T1, T2, T3, T4, T5, T6, T7> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<VoidType> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> implementation, Times? times = null)
    {
        return new Behavior<VoidType>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<TOut>(
        Func<TOut?> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T, TOut>(
        Func<T, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, TOut>(
        Func<T1, T2, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, TOut>(
        Func<T1, T2, T3, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, TOut>(
        Func<T1, T2, T3, T4, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, TOut>(
        Func<T1, T2, T3, T4, T5, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, TOut>(
        Func<T1, T2, T3, T4, T5, T6, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <inheritdoc cref="Set{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }

    /// <summary>Specifies mock behavior of <paramref name="implementation"/> for a fake.</summary>
    /// <typeparam name="T1">Type for <paramref name="implementation"/> parameter 1.</typeparam>
    /// <typeparam name="T2">Type for <paramref name="implementation"/> parameter 2.</typeparam>
    /// <typeparam name="T3">Type for <paramref name="implementation"/> parameter 3.</typeparam>
    /// <typeparam name="T4">Type for <paramref name="implementation"/> parameter 4.</typeparam>
    /// <typeparam name="T5">Type for <paramref name="implementation"/> parameter 5.</typeparam>
    /// <typeparam name="T6">Type for <paramref name="implementation"/> parameter 6.</typeparam>
    /// <typeparam name="T7">Type for <paramref name="implementation"/> parameter 7.</typeparam>
    /// <typeparam name="T8">Type for <paramref name="implementation"/> parameter 8.</typeparam>
    /// <typeparam name="T9">Type for <paramref name="implementation"/> parameter 9.</typeparam>
    /// <typeparam name="T10">Type for <paramref name="implementation"/> parameter 10.</typeparam>
    /// <typeparam name="T11">Type for <paramref name="implementation"/> parameter 11.</typeparam>
    /// <typeparam name="T12">Type for <paramref name="implementation"/> parameter 12.</typeparam>
    /// <typeparam name="T13">Type for <paramref name="implementation"/> parameter 13.</typeparam>
    /// <typeparam name="T14">Type for <paramref name="implementation"/> parameter 14.</typeparam>
    /// <typeparam name="T15">Type for <paramref name="implementation"/> parameter 15.</typeparam>
    /// <typeparam name="T16">Type for <paramref name="implementation"/> parameter 16.</typeparam>
    /// <typeparam name="TOut">Type for the return value of <paramref name="implementation"/>.</typeparam>
    /// <param name="implementation">Behavior to invoke for calls.</param>
    /// <param name="times">Behavior call limit.</param>
    /// <returns>Instance to set up the mock with.</returns>
    public static Behavior<TOut> Set<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut> implementation, Times? times = null)
    {
        return new Behavior<TOut>(implementation, times);
    }
}
