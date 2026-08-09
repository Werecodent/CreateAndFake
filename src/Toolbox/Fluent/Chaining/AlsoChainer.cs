using System.Collections;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Fluent.Chaining;

/// <summary>Chainer enabling additional assertion calls.</summary>
/// <param name="asserter">Configured options for <see langword="this"/>.</param>
public class AlsoChainer(IAsserter asserter)
{
    /// <inheritdoc cref="AssertAsyncEnumerable{T}"/>
    /// <param name="collection"><inheritdoc cref="AssertAsyncEnumerableBase{T,T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public AssertAsyncEnumerable<T> Also<T>(IAsyncEnumerable<T> collection)
    {
        return new AssertAsyncEnumerable<T>(asserter, collection);
    }

    /// <inheritdoc cref="AssertObject"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public AssertAsyncObject Also(object? actual)
    {
        return new AssertAsyncObject(asserter, actual);
    }

    /// <inheritdoc cref="AssertGenericTask{T}"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public AssertGenericTask<T> Also<T>(Task<T>? behavior)
    {
        return new AssertGenericTask<T>(asserter, behavior);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public AssertGenericValueTask<T> Also<T>(ValueTask<T>? actual)
    {
        return new AssertGenericValueTask<T>(asserter, actual);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public AssertGenericValueTask<T> Also<T>(ValueTask<T> actual)
    {
        return new AssertGenericValueTask<T>(asserter, actual);
    }

    /// <inheritdoc cref="AssertTask"/>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public AssertTask Also(Task? behavior)
    {
        return new AssertTask(asserter, behavior);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public AssertValueTask Also(ValueTask actual)
    {
        return new AssertValueTask(asserter, actual);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public AssertValueTask Also(ValueTask? actual)
    {
        return new AssertValueTask(asserter, actual);
    }

    /// <inheritdoc cref="AssertDelegate"/>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public AssertAction Also(Action? behavior)
    {
        return new AssertAction(asserter, behavior);
    }

    /// <inheritdoc cref="AssertComparable"/>
    /// <param name="value"><inheritdoc cref="AssertComparableBase{T}.Value" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="value"/> with.</returns>
    public AssertComparable Also(IComparable? value)
    {
        return new AssertComparable(asserter, value);
    }

    /// <inheritdoc cref="AssertEnumerable"/>
    /// <param name="collection"><inheritdoc cref="AssertEnumerableBase{T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public AssertEnumerable Also(IEnumerable? collection)
    {
        return new AssertEnumerable(asserter, collection);
    }

    /// <inheritdoc cref="AssertError"/>
    /// <param name="error"><inheritdoc cref="AssertErrorBase{T}.Error" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="error"/> with.</returns>
    public AssertError Also(Exception? error)
    {
        return new AssertError(asserter, error);
    }

    /// <inheritdoc cref="AssertDelegate"/>
    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public AssertFunc<T> Also<T>(Func<T>? behavior)
    {
        return new AssertFunc<T>(asserter, behavior);
    }

    /// <inheritdoc cref="AssertString"/>
    /// <param name="text"><inheritdoc cref="AssertStringBase{T}.Text" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="text"/> with.</returns>
    public AssertString Also(string? text)
    {
        return new AssertString(asserter, text);
    }

    /// <inheritdoc cref="AssertType"/>
    /// <param name="type"><inheritdoc cref="AssertTypeBase{T}.Type" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="type"/> with.</returns>
    public AssertType Also(Type? type)
    {
        return new AssertType(asserter, type);
    }
}
