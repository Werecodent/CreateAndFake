using System.Collections;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class AssertExtensions
{
    /// <inheritdoc cref="AssertAsyncEnumerable{T}"/>
    /// <param name="collection"><inheritdoc cref="AssertAsyncEnumerableBase{T,T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static AssertAsyncEnumerable<T> Assert<T>(this IAsyncEnumerable<T> collection)
    {
        return new AssertAsyncEnumerable<T>(Tools.Asserter, collection);
    }

    /// <inheritdoc cref="AssertObject"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertAsyncObject Assert(this object? actual)
    {
        return new AssertAsyncObject(Tools.Asserter, actual);
    }

    /// <inheritdoc cref="AssertGenericTask{T}"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertGenericTask<T> Assert<T>(this Task<T>? behavior)
    {
        return new AssertGenericTask<T>(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertGenericValueTask<T> Assert<T>(this ValueTask<T>? actual)
    {
        return new AssertGenericValueTask<T>(Tools.Asserter, actual);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertGenericValueTask<T> Assert<T>(this ValueTask<T> actual)
    {
        return new AssertGenericValueTask<T>(Tools.Asserter, actual);
    }

    /// <inheritdoc cref="AssertTask"/>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertTask Assert(this Task? behavior)
    {
        return new AssertTask(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertValueTask Assert(this ValueTask actual)
    {
        return new AssertValueTask(Tools.Asserter, actual);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertValueTask Assert(this ValueTask? actual)
    {
        return new AssertValueTask(Tools.Asserter, actual);
    }

    /// <inheritdoc cref="AssertDelegate"/>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertAction Assert(this Action? behavior)
    {
        return new AssertAction(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertComparable"/>
    /// <param name="value"><inheritdoc cref="AssertComparableBase{T}.Value" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="value"/> with.</returns>
    public static AssertComparable Assert(this IComparable? value)
    {
        return new AssertComparable(Tools.Asserter, value);
    }

    /// <inheritdoc cref="AssertEnumerable"/>
    /// <param name="collection"><inheritdoc cref="AssertEnumerableBase{T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static AssertEnumerable Assert(this IEnumerable? collection)
    {
        return new AssertEnumerable(Tools.Asserter, collection);
    }

    /// <inheritdoc cref="AssertError"/>
    /// <param name="error"><inheritdoc cref="AssertErrorBase{T}.Error" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="error"/> with.</returns>
    public static AssertError Assert(this Exception? error)
    {
        return new AssertError(Tools.Asserter, error);
    }

    /// <inheritdoc cref="AssertDelegate"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertFunc<T> Assert<T>(this Func<T>? behavior)
    {
        return new AssertFunc<T>(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertString"/>
    /// <param name="text"><inheritdoc cref="AssertStringBase{T}.Text" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="text"/> with.</returns>
    public static AssertString Assert(this string? text)
    {
        return new AssertString(Tools.Asserter, text);
    }

    /// <inheritdoc cref="AssertType"/>
    /// <param name="type"><inheritdoc cref="AssertTypeBase{T}.Type" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="type"/> with.</returns>
    public static AssertType Assert(this Type? type)
    {
        return new AssertType(Tools.Asserter, type);
    }

    /// <summary>Handles assertion calls for runtime <paramref name="behavior"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> of <paramref name="origin"/>.</typeparam>
    /// <param name="origin">Object with <paramref name="behavior"/> to test.</param>
    /// <param name="behavior"><c>Delegate</c> on <paramref name="origin"/> to test.</param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    /// <remarks>Primarily useful for exception testing.</remarks>
    public static AssertAction Assert<T>(this T origin, Action<T> behavior)
    {
        return Assert(() => behavior.Invoke(origin));
    }

    /// <inheritdoc cref="Assert{T}(T,Action{T})"/>
    public static AssertFunc<TResult> Assert<TSelf, TResult>(
        this TSelf origin,
        Func<TSelf, TResult> behavior
    )
    {
        return Assert(() => behavior.Invoke(origin));
    }
}
