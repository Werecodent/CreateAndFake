using System.Collections;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskWithChainerExtensions
{
    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertAsyncEnumerable<TContent>> With<T, TContent>(
        this Task<ResultChainer<T>> origin,
        Func<T, IAsyncEnumerable<TContent>> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertAsyncObject> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, object?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertGenericTask<TContent>> With<T, TContent>(
        this Task<ResultChainer<T>> origin,
        Func<T, Task<TContent>?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertGenericValueTask<TContent>> With<T, TContent>(
        this Task<ResultChainer<T>> origin,
        Func<T, ValueTask<TContent>?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertGenericValueTask<TContent>> With<T, TContent>(
        this Task<ResultChainer<T>> origin,
        Func<T, ValueTask<TContent>> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertTask> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, Task?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertValueTask> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, ValueTask> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertValueTask> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, ValueTask?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertAction> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, Action?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertAction> With<T>(
        this Task<ResultChainer<T>> origin,
        Action<T>? selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertComparable> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, IComparable?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertDelegate> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, Delegate?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertEnumerable> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, IEnumerable?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertError> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, Exception?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <param name="origin">Assert provider in asynchronous context.</param>
    /// <inheritdoc cref="WithChainer{T}.With{TContent}(Func{T,Func{TContent}})"/>
    /// <inheritdoc cref="WithChainer{T}"/>
    public static async Task<AssertFunc<TContent>> With<T, TContent>(
        this Task<ResultChainer<T>> origin,
        Func<T, Func<TContent>?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }

    /// <inheritdoc cref="With{T,TContent}(Task{ResultChainer{T}},Func{T,Func{TContent}})"/>
    public static async Task<AssertString> With<T>(
        this Task<ResultChainer<T>> origin,
        Func<T, string?> selector
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).With(selector);
    }
}
