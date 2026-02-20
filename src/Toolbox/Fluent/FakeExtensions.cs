using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.Fluent;

#pragma warning disable IDE0060, RCS1175 // Provides fluent attachment.

/// <summary>Provides fluent handling of fakes.</summary>
public static class FakeExtensions
{
    /// <summary>Ties a method call to fake behavior.</summary>
    /// <typeparam name="T"></typeparam>
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
    /// <typeparam name="T"></typeparam>
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
}

#pragma warning restore IDE0060, RCS1175
