using System.Collections;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class AssertExtensions
{
    private static IAsserter GetAsserter(ToolSet? tools)
    {
        return tools?.Asserter ?? Tools.Asserter;
    }

    /// <inheritdoc cref="AssertAsyncEnumerable{T}"/>
    /// <param name="collection"><inheritdoc cref="AssertAsyncEnumerableBase{T,T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static AssertAsyncEnumerable<T> Assert<T>(
        this IAsyncEnumerable<T> collection,
        ToolSet? tools = null
    )
    {
        return new AssertAsyncEnumerable<T>(GetAsserter(tools), collection);
    }

    /// <inheritdoc cref="AssertObject"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertAsyncObject Assert(this object? actual, ToolSet? tools = null)
    {
        return new AssertAsyncObject(GetAsserter(tools), actual);
    }

    /// <inheritdoc cref="AssertGenericTask{T}"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertGenericTask<T> Assert<T>(this Task<T>? behavior, ToolSet? tools = null)
    {
        return new AssertGenericTask<T>(GetAsserter(tools), behavior);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertGenericValueTask<T> Assert<T>(
        this ValueTask<T>? actual,
        ToolSet? tools = null
    )
    {
        return new AssertGenericValueTask<T>(GetAsserter(tools), actual);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertGenericValueTask<T> Assert<T>(
        this ValueTask<T> actual,
        ToolSet? tools = null
    )
    {
        return new AssertGenericValueTask<T>(GetAsserter(tools), actual);
    }

    /// <inheritdoc cref="AssertTask"/>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertTask Assert(this Task? behavior, ToolSet? tools = null)
    {
        return new AssertTask(GetAsserter(tools), behavior);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertValueTask Assert(this ValueTask actual, ToolSet? tools = null)
    {
        return new AssertValueTask(GetAsserter(tools), actual);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertValueTask Assert(this ValueTask? actual, ToolSet? tools = null)
    {
        return new AssertValueTask(GetAsserter(tools), actual);
    }

    /// <inheritdoc cref="AssertDelegate"/>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertAction Assert(this Action? behavior, ToolSet? tools = null)
    {
        return new AssertAction(GetAsserter(tools), behavior);
    }

    /// <inheritdoc cref="AssertComparable"/>
    /// <param name="value"><inheritdoc cref="AssertComparableBase{T}.Value" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="value"/> with.</returns>
    public static AssertComparable Assert(this IComparable? value, ToolSet? tools = null)
    {
        return new AssertComparable(GetAsserter(tools), value);
    }

    /// <inheritdoc cref="AssertEnumerable"/>
    /// <param name="collection"><inheritdoc cref="AssertEnumerableBase{T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static AssertEnumerable Assert(this IEnumerable? collection, ToolSet? tools = null)
    {
        return new AssertEnumerable(GetAsserter(tools), collection);
    }

    /// <inheritdoc cref="AssertError"/>
    /// <param name="error"><inheritdoc cref="AssertErrorBase{T}.Error" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="error"/> with.</returns>
    public static AssertError Assert(this Exception? error, ToolSet? tools = null)
    {
        return new AssertError(GetAsserter(tools), error);
    }

    /// <inheritdoc cref="AssertDelegate"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertFunc<T> Assert<T>(this Func<T>? behavior, ToolSet? tools = null)
    {
        return new AssertFunc<T>(GetAsserter(tools), behavior);
    }

    /// <inheritdoc cref="AssertString"/>
    /// <param name="text"><inheritdoc cref="AssertStringBase{T}.Text" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="text"/> with.</returns>
    public static AssertString Assert(this string? text, ToolSet? tools = null)
    {
        return new AssertString(GetAsserter(tools), text);
    }

    /// <inheritdoc cref="AssertType"/>
    /// <param name="type"><inheritdoc cref="AssertTypeBase{T}.Type" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="type"/> with.</returns>
    public static AssertType Assert(this Type? type, ToolSet? tools = null)
    {
        return new AssertType(GetAsserter(tools), type);
    }

    /// <summary>Handles assertion calls for runtime <paramref name="behavior"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> of <paramref name="origin"/>.</typeparam>
    /// <param name="origin">Object with <paramref name="behavior"/> to test.</param>
    /// <param name="behavior"><c>Delegate</c> on <paramref name="origin"/> to test.</param>
    /// <param name="tools">Asserter provider to use.</param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    /// <remarks>Primarily useful for exception testing.</remarks>
    public static AssertAction Assert<T>(this T origin, Action<T> behavior, ToolSet? tools = null)
    {
        return Assert(() => behavior.Invoke(origin), tools);
    }

    /// <inheritdoc cref="Assert{T}(T,Action{T},ToolSet)"/>
    public static AssertFunc<TResult> Assert<TSelf, TResult>(
        this TSelf origin,
        Func<TSelf, TResult> behavior,
        ToolSet? tools = null
    )
    {
        return Assert(() => behavior.Invoke(origin), tools);
    }
}
