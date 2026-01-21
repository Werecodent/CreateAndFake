using System.Collections;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class AssertExtensions
{
    /// <inheritdoc cref="AssertObject"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static AssertAsyncObject Assert(this object? actual)
    {
        return new AssertAsyncObject(Tools.Asserter, actual);
    }

    /// <inheritdoc cref="AssertEnumerable"/>
    /// <param name="collection"><inheritdoc cref="AssertEnumerableBase{T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static AssertEnumerable Assert(this IEnumerable? collection)
    {
        return new AssertEnumerable(Tools.Asserter, collection);
    }

    /// <inheritdoc cref="AssertAsyncEnumerable{T}"/>
    /// <param name="collection"><inheritdoc cref="AssertAsyncEnumerableBase{T,T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static AssertAsyncEnumerable<T> Assert<T>(this IAsyncEnumerable<T> collection)
    {
        return new AssertAsyncEnumerable<T>(Tools.Asserter, collection);
    }

    /// <inheritdoc cref="AssertString"/>
    /// <param name="text"><inheritdoc cref="AssertStringBase{T}.Text" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="text"/> with.</returns>
    public static AssertString Assert(this string? text)
    {
        return new AssertString(Tools.Asserter, text);
    }

    /// <inheritdoc cref="AssertComparable"/>
    /// <param name="value"><inheritdoc cref="AssertComparableBase{T}.Value" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="value"/> with.</returns>
    public static AssertComparable Assert(this IComparable? value)
    {
        return new AssertComparable(Tools.Asserter, value);
    }

    /// <inheritdoc cref="AssertType"/>
    /// <param name="type"><inheritdoc cref="AssertTypeBase{T}.Type" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="type"/> with.</returns>
    public static AssertType Assert(this Type? type)
    {
        return new AssertType(Tools.Asserter, type);
    }

    /// <inheritdoc cref="AssertError"/>
    /// <param name="error"><inheritdoc cref="AssertErrorBase{T}.Error" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="error"/> with.</returns>
    public static AssertError Assert(this Exception? error)
    {
        return new AssertError(Tools.Asserter, error);
    }

    /// <inheritdoc cref="AssertBehavior"/>
    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertBehavior Assert(this Action? behavior)
    {
        return new AssertBehavior(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertBehavior"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertBehavior Assert<T>(this Func<T>? behavior)
    {
        return new AssertBehavior(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertAsync"/>
    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertAsync Assert(this Func<Task>? behavior)
    {
        return new AssertAsync(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertAsync"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertAsync Assert<T>(this Func<Task<T?>>? behavior)
    {
        return new AssertAsync(Tools.Asserter, behavior);
    }

    /// <inheritdoc cref="AssertAsync"/>
    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertAsync Assert(this Task? behavior)
    {
        return new AssertAsync(Tools.Asserter, () => behavior);
    }

    /// <inheritdoc cref="AssertAsync"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertBehaviorBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static AssertAsync Assert<T>(this Task<T?>? behavior)
    {
        return new AssertAsync(Tools.Asserter, () => behavior);
    }

    /// <summary>Handles assertion calls for runtime <paramref name="behavior"/>.</summary>
    /// <typeparam name="T"><see cref="Type"/> of <paramref name="origin"/>.</typeparam>
    /// <param name="origin">Object with <paramref name="behavior"/> to test.</param>
    /// <param name="behavior"><c>Delegate</c> on <paramref name="origin"/> to test.</param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    /// <remarks>Primarily useful for exception testing.</remarks>
    public static AssertBehavior Assert<T>(this T origin, Action<T> behavior)
    {
        return Assert(() => behavior.Invoke(origin));
    }

    /// <inheritdoc cref="Assert{T}(T,Action{T})"/>
    public static AssertBehavior Assert<T>(this T origin, Func<T, object> behavior)
    {
        return Assert(() => behavior.Invoke(origin));
    }

    /// <inheritdoc cref="Assert{T}(T,Action{T})"/>
    public static AssertAsync Assert<T>(this T origin, Func<T, Task> behavior)
    {
        return Assert(() => behavior.Invoke(origin));
    }

    /// <inheritdoc cref="Assert{T}(T,Action{T})"/>
    public static AssertAsync Assert<T>(this T origin, Func<T, Task<object?>> behavior)
    {
        return Assert(() => behavior.Invoke(origin));
    }
}
