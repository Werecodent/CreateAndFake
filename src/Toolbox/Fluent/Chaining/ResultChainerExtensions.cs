using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

#pragma warning disable IDE0130 // Cleaner project organization.

namespace CreateAndFake.Fluent;

#pragma warning restore

/// <summary>Provides fluent assertions.</summary>
public static class ResultChainerExtensions
{
    /// <inheritdoc cref="AlsoChainer.Also{T}(IAsyncEnumerable{T})"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertAsyncEnumerable<T> That<T>(this ResultChainer<IAsyncEnumerable<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(object)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertAsyncObject That(this ResultChainer<object?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(Task{T})"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertGenericTask<T> That<T>(this ResultChainer<Task<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(ValueTask{T}?)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertGenericValueTask<T> That<T>(this ResultChainer<ValueTask<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(ValueTask{T})"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertGenericValueTask<T> That<T>(this ResultChainer<ValueTask<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(Task)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertTask That(this ResultChainer<Task?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(ValueTask)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertValueTask That(this ResultChainer<ValueTask> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(ValueTask?)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertValueTask That(this ResultChainer<ValueTask?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(Action)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertAction That(this ResultChainer<Action?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(IComparable)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertComparable That(this ResultChainer<IComparable?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(IEnumerable)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertEnumerable That(this ResultChainer<IEnumerable?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(Exception)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertError That(this ResultChainer<Exception?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(Func{T})"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertFunc<T> That<T>(this ResultChainer<Func<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(string)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertString That(this ResultChainer<string?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also(Type)"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertType That(this ResultChainer<Type?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }
}
