namespace CreateAndFake.Toolbox.FakerTool;

/// <summary>Creates fake objects.</summary>
public interface IFaker
{
    /// <summary>Configured options for <c>this</c>.</summary>
    FakerOptions Options { get; }

    /// <summary>Determines if type <typeparamref name="T"/> can be faked.</summary>
    /// <typeparam name="T"><c>Type</c> to check.</typeparam>
    /// <returns><c>true</c> if possible; <c>false</c> otherwise.</returns>
    bool Supports<T>();

    /// <summary>Determines if <paramref name="type"/> can be faked.</summary>
    /// <param name="type"><c>Type</c> to check.</param>
    /// <returns><c>true</c> if possible; <c>false</c> otherwise.</returns>
    bool Supports(Type type);

    /// <typeparam name="T"><c>Type</c> being faked.</typeparam>
    /// <inheritdoc cref="Mock"/>
    Fake<T> Mock<T>(params Type[] interfaces);

    /// <summary>Creates a strict fake where calls fail unless set up.</summary>
    /// <param name="parent">Type being faked.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>Handler for fake behavior.</returns>
    Fake Mock(Type parent, params Type[] interfaces);

    /// <typeparam name="T"><c>Type</c> being faked.</typeparam>
    /// <inheritdoc cref="Stub"/>
    Fake<T> Stub<T>(params Type[] interfaces);

    /// <summary>Creates a loose fake with a base default implementation.</summary>
    /// <param name="parent">Type being faked.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>Handler for the fake behavior.</returns>
    Fake Stub(Type parent, params Type[] interfaces);

    /// <summary>Creates an instance injected with mocks.</summary>
    /// <inheritdoc cref="InjectStubs"/>
    Injected<T> InjectMocks<T>(params object[] values);

    /// <summary>Creates an instance injected with stubs.</summary>
    /// <typeparam name="T">Instance type to be created.</typeparam>
    /// <param name="values">Values to inject instead where possible.</param>
    /// <returns>The created instance with its fakes.</returns>
    Injected<T> InjectStubs<T>(params object[] values);
}
