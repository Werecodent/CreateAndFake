using System.Collections;
using CreateAndFake.AsserterTool;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent.Chaining;

/// <summary>Chainer enabling additional assertion calls.</summary>
/// <param name="asserter">Configured options for <see langword="this"/>.</param>
public class AlsoChainer(IAsserter asserter)
{
    /// <param name="behavior"><inheritdoc cref="AssertActionBase{T}.Action" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertAction Also(Action? behavior)
    {
        return new AssertAction(asserter, behavior);
    }

    /// <param name="value"><inheritdoc cref="AssertComparableBase{T}.Value" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="value"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertComparable Also(IComparable? value)
    {
        return new AssertComparable(asserter, value);
    }

    /// <param name="behavior"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertDelegate Also(Delegate? behavior)
    {
        return new AssertDelegate(asserter, behavior);
    }

    /// <param name="collection"><inheritdoc cref="AssertEnumerableBase{T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertEnumerable Also(IEnumerable? collection)
    {
        return new AssertEnumerable(asserter, collection);
    }

    /// <param name="error"><inheritdoc cref="AssertErrorBase{T}.Error" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="error"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertError Also(Exception? error)
    {
        return new AssertError(asserter, error);
    }

    /// <typeparam name="T">Return <see cref="Type"/> of <paramref name="behavior"/>.</typeparam>
    /// <param name="behavior"><inheritdoc cref="AssertFuncBase{T,T}.Function" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertFunc<T> Also<T>(Func<T>? behavior)
    {
        return new AssertFunc<T>(asserter, behavior);
    }

    /// <summary>Specifies a different instance to test.</summary>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public AssertObject Also(object? actual)
    {
        return new AssertObject(asserter, actual);
    }

    /// <param name="text"><inheritdoc cref="AssertStringBase{T}.Text" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="text"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertString Also(string? text)
    {
        return new AssertString(asserter, text);
    }

    /// <param name="type"><inheritdoc cref="AssertTypeBase{T}.Type" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="type"/> with.</returns>
    /// <inheritdoc cref="Also(object)"/>
    public AssertType Also(Type? type)
    {
        return new AssertType(asserter, type);
    }
}
