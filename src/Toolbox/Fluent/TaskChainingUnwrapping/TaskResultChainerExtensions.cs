using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskResultChainerExtensions
{
    /// <inheritdoc cref="ResultChainer{T}.With"/>
    public static async Task<T> GetResultValue<T>(this Task<ResultChainer<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GetResultValue();
    }

    /// <inheritdoc cref="AlsoChainer.Also(object)"/>
    public static async Task<AssertAsyncObject> That<T>(this Task<ResultChainer<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        ResultChainer<T> chainer = await origin.ConfigureAwait(false);
        return chainer.Also(chainer.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(IAsyncEnumerable{T})"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertAsyncEnumerable<T>> That<T>(
        this Task<ResultChainer<IAsyncEnumerable<T>>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(object)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertAsyncObject> That(this Task<ResultChainer<object?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(Task{T})"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertGenericTask<T>> That<T>(
        this Task<ResultChainer<Task<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(ValueTask{T}?)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertGenericValueTask<T>> That<T>(
        this Task<ResultChainer<ValueTask<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(ValueTask{T})"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertGenericValueTask<T>> That<T>(
        this Task<ResultChainer<ValueTask<T>>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(Task)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertTask> That(this Task<ResultChainer<Task?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(ValueTask)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertValueTask> That(this Task<ResultChainer<ValueTask>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(ValueTask?)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertValueTask> That(this Task<ResultChainer<ValueTask?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(Action)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertAction> That(this Task<ResultChainer<Action?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(IComparable)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertComparable> That(this Task<ResultChainer<IComparable?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(IEnumerable)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<IEnumerable?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(Exception)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertError> That(this Task<ResultChainer<Exception?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(Func{T})"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertFunc<T>> That<T>(this Task<ResultChainer<Func<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertString> That(this Task<ResultChainer<string?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also(Type)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertType> That(this Task<ResultChainer<Type?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }
}
