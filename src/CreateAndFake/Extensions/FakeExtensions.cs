using CreateAndFake.Design;
using CreateAndFake.Toolbox.AsserterTool.Fluent;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;

#pragma warning disable IDE0060 // Remove unused parameter: Provides fluent attachment.

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent handling of fakes.</summary>
public static class FakeExtensions
{
    /// <summary>Ties a method call to fake behavior.</summary>
    /// <param name="fakeCallResult">Result from the fake method to setup.</param>
    /// <param name="returnValue">Value to set the call behavior with.</param>
    /// <param name="times">Expected number of calls for the behavior.</param>
    /// <remarks>For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.</remarks>
    /// <example>
    ///     <code>
    ///         T db = Tools.Faker.Stub{T}().Dummy;
    ///         db.Find(id).SetupReturn(data);
    ///     </code>
    /// </example>
    public static void SetupReturn<T>(this T fakeCallResult, T returnValue, Times? times = null)
    {
        SetupCall(fakeCallResult, Behavior.Returns(returnValue, times));
    }

    /// <summary>Ties a method call to <paramref name="behavior"/>.</summary>
    /// <param name="fakeCallResult">Result from the fake method to setup.</param>
    /// <param name="behavior">Behavior to set the call behavior with.</param>
    /// <remarks>For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.</remarks>
    public static void SetupCall<T>(this T fakeCallResult, Behavior<T> behavior)
    {
        FakeMetaProvider.SetLastCallBehavior(behavior);
    }

    /// <summary>Accesses the raw fake wrapper.</summary>
    /// <typeparam name="T">Faked type.</typeparam>
    /// <param name="fakeDummy">Fake instance to wrap.</param>
    /// <returns>Fake to test with.</returns>
    /// <remarks>For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.</remarks>
    public static Fake<T> ToFake<T>(this T fakeDummy)
    {
        return new((IFaked)fakeDummy!);
    }

    /// <summary>Accesses the raw fake wrapper.</summary>
    /// <typeparam name="T">Faked type to cast to.</typeparam>
    /// <param name="fakeDummy">Fake instance to wrap.</param>
    /// <returns>Fake to test with.</returns>
    /// <remarks>For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.</remarks>
    public static Fake<T> ToFake<T>(this object fakeDummy)
    {
        return new((IFaked)fakeDummy);
    }

    /// <summary>Verifies all behaviors with associated times were called as expected.</summary>
    /// <param name="fake">Fake instance with behavior set.</param>
    /// <param name="total">Expected total number of calls to test as well.</param>
    /// <remarks>
    ///     For use on <see cref="IFaked"/> stubs from the <see cref="Faker"/> tool only.
    ///     When specifying <paramref name="total"/>, be aware of test framework calls for info/display.
    /// </remarks>
    public static void VerifyAllCalls(this object fake, Times? total = null)
    {
        new Fake((IFaked)fake).VerifyAll(total);
    }

    /// <param name="asserter">Asserter testing a method call using fakes.</param>
    /// <param name="fake">Fake instance with behavior set.</param>
    /// <param name="total">Expected total number of calls to test as well.</param>
    /// <inheritdoc cref="VerifyAllCalls"/>
    public static AssertChainer<AssertObject> Called(this AssertObject asserter, object fake, Times? total = null)
    {
        ArgumentGuard.ThrowIfNull(asserter, nameof(asserter));

        VerifyAllCalls(fake, total);
        return asserter.ToChainer();
    }

    /// <inheritdoc cref="Called(AssertObject,object,Times)"/>
    public static AssertChainer<AssertEnumerable> Called(this AssertEnumerable asserter, object fake, Times? total = null)
    {
        ArgumentGuard.ThrowIfNull(asserter, nameof(asserter));

        VerifyAllCalls(fake, total);
        return asserter.ToChainer();
    }

    /// <inheritdoc cref="Called(AssertObject,object,Times)"/>
    public static AssertChainer<AssertBehavior> Called(this AssertBehavior asserter, object fake, Times? total = null)
    {
        ArgumentGuard.ThrowIfNull(asserter, nameof(asserter));

        VerifyAllCalls(fake, total);
        return asserter.ToChainer();
    }

    /// <inheritdoc cref="Called(AssertObject,object,Times)"/>
    public static AssertChainer<AssertString> Called(this AssertString asserter, object fake, Times? total = null)
    {
        ArgumentGuard.ThrowIfNull(asserter, nameof(asserter));

        VerifyAllCalls(fake, total);
        return asserter.ToChainer();
    }

    /// <inheritdoc cref="Called(AssertObject,object,Times)"/>
    public static AssertChainer<AssertComparable> Called(this AssertComparable asserter, object fake, Times? total = null)
    {
        ArgumentGuard.ThrowIfNull(asserter, nameof(asserter));

        VerifyAllCalls(fake, total);
        return asserter.ToChainer();
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
