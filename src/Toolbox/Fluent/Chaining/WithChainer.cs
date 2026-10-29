using System.Collections;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Fluent.Chaining;

/// <param name="result"><inheritdoc cref="Result" path="/summary"/></param>
/// <typeparam name="T">Result <see cref="Type"/> to chain.</typeparam>
/// <inheritdoc cref="AlsoChainer"/>
public abstract class WithChainer<T>(T result, IAsserter asserter) : AlsoChainer(asserter)
{
    /// <summary>Base instance to chain with more assertions.</summary>
    protected T Result { get; } = result;

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertAsyncEnumerable<TContent> With<TContent>(
        Func<T, IAsyncEnumerable<TContent>> selector
    )
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertAsyncObject With(Func<T, object?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertGenericTask<TContent> With<TContent>(Func<T, Task<TContent>?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertGenericValueTask<TContent> With<TContent>(Func<T, ValueTask<TContent>?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertGenericValueTask<TContent> With<TContent>(Func<T, ValueTask<TContent>> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertTask With(Func<T, Task?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertValueTask With(Func<T, ValueTask> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertValueTask With(Func<T, ValueTask?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertAction With(Func<T, Action?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertAction With(Action<T>? selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(() => selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertComparable With(Func<T, IComparable?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertDelegate With(Func<T, Delegate?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertEnumerable With(Func<T, IEnumerable?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertError With(Func<T, Exception?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <summary>Begins assertions on an associated value.</summary>
    /// <typeparam name="TContent">Associated content type.</typeparam>
    /// <param name="selector">How to retrieve the value to test on.</param>
    /// <returns>Asserter to test the selected value.</returns>
    public AssertFunc<TContent> With<TContent>(Func<T, Func<TContent>?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }

    /// <inheritdoc cref="With{TContent}(Func{T,Func{TContent}})"/>
    public AssertString With(Func<T, string?> selector)
    {
        ArgumentGuard.ThrowIfNull(selector);
        return Also(selector.Invoke(Result));
    }
}
