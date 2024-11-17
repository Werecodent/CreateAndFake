global using FakerMod = System.Func<
    CreateAndFake.Toolbox.FakerTool.FakerOptions,
    CreateAndFake.Toolbox.FakerTool.FakerOptions>;
namespace CreateAndFake.Toolbox.FakerTool;

/// <summary>Creates fake objects.</summary>
public interface IFaker
{
    /// <summary>Configured options for <c>this</c>.</summary>
    FakerOptions Options { get; }

    /// <inheritdoc cref="Supports{T}(FakerMod)"/>
    bool Supports<T>();

    /// <summary>Determines if type <typeparamref name="T"/> can be faked.</summary>
    /// <typeparam name="T"><c>Type</c> to check.</typeparam>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns><c>true</c> if possible; <c>false</c> otherwise.</returns>
    bool Supports<T>(FakerMod? optionConfiguration);

    /// /// <inheritdoc cref="Supports(Type,FakerMod)"/>
    bool Supports(Type type);

    /// <summary>Determines if <paramref name="type"/> can be faked.</summary>
    /// <param name="type"><c>Type</c> to check.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <returns><c>true</c> if possible; <c>false</c> otherwise.</returns>
    bool Supports(Type type, FakerMod? optionConfiguration);

    /// /// <inheritdoc cref="Mock{T}(FakerMod,IEnumerable{Type})"/>
    Fake<T> Mock<T>(params IEnumerable<Type> interfaces);

    /// <typeparam name="T"><c>Type</c> being faked.</typeparam>
    /// <inheritdoc cref="Mock(Type,FakerMod,IEnumerable{Type})"/>
    Fake<T> Mock<T>(FakerMod? optionConfiguration, params IEnumerable<Type> interfaces);

    /// <inheritdoc cref="Mock(Type,FakerMod,IEnumerable{Type})"/>
    Fake Mock(Type parent, params IEnumerable<Type> interfaces);

    /// <summary>Creates a strict fake where calls fail unless set up.</summary>
    /// <param name="parent">Type being faked.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>Handler for fake behavior.</returns>
    Fake Mock(Type parent, FakerMod? optionConfiguration, params IEnumerable<Type> interfaces);

    /// /// <inheritdoc cref="Stub{T}(FakerMod,IEnumerable{Type})"/>
    Fake<T> Stub<T>(params IEnumerable<Type> interfaces);

    /// <typeparam name="T"><c>Type</c> being faked.</typeparam>
    /// <inheritdoc cref="Stub(Type,FakerMod,IEnumerable{Type})"/>
    Fake<T> Stub<T>(FakerMod? optionConfiguration, params IEnumerable<Type> interfaces);

    /// <inheritdoc cref="Stub(Type,FakerMod,IEnumerable{Type})"/>
    Fake Stub(Type parent, params IEnumerable<Type> interfaces);

    /// <summary>Creates a loose fake with a base default implementation.</summary>
    /// <param name="parent">Type being faked.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>Handler for the fake behavior.</returns>
    Fake Stub(Type parent, FakerMod? optionConfiguration, params IEnumerable<Type> interfaces);

    /// <inheritdoc cref="InjectMocks{T}(FakerMod,IEnumerable{object})"/>
    Injected<T> InjectMocks<T>(params IEnumerable<object> values);

    /// <summary>Creates an instance injected with mocks.</summary>
    /// <inheritdoc cref="InjectStubs{T}(FakerMod,IEnumerable{object})"/>
    Injected<T> InjectMocks<T>(FakerMod? optionConfiguration, params IEnumerable<object> values);

    /// <inheritdoc cref="InjectStubs{T}(FakerMod,IEnumerable{object})"/>
    Injected<T> InjectStubs<T>(params IEnumerable<object> values);

    /// <summary>Creates an instance injected with stubs.</summary>
    /// <typeparam name="T">Instance type to be created.</typeparam>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <param name="values">Values to inject instead where possible.</param>
    /// <returns>The created instance with its fakes.</returns>
    Injected<T> InjectStubs<T>(FakerMod? optionConfiguration, params IEnumerable<object> values);
}
