global using FakerMod = System.Func<
    Werecodent.CreateAndFake.FakerTool.FakerOptions,
    Werecodent.CreateAndFake.FakerTool.FakerOptions
>;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.FakerTool;

/// <summary>Creates fake objects.</summary>
public interface IFaker : IHintTool<FakerOptions, IFakeHint>
{
    /// <summary>Creates a new tool with the given configuration changes.</summary>
    /// <param name="optionConfiguration">Modifications of Options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IFaker WithOptions(FakerMod optionConfiguration);

    /// <summary>Determines if type <typeparamref name="T"/> can be faked.</summary>
    /// <typeparam name="T"><see cref="Type"/> to check.</typeparam>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns><see langword="true"/> if possible, <see langword="false"/> otherwise.</returns>
    bool Supports<T>(FakerMod? optionConfiguration = null);

    /// <summary>Determines if <paramref name="type"/> can be faked.</summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns><see langword="true"/> if possible, <see langword="false"/> otherwise.</returns>
    bool Supports(Type type, FakerMod? optionConfiguration = null);

    /// /// <inheritdoc cref="Mock{T}(IEnumerable{Type},FakerMod)"/>
    Fake<T> Mock<T>(params IEnumerable<Type> interfaces);

    /// <typeparam name="T"><see cref="Type"/> being faked.</typeparam>
    /// <inheritdoc cref="Mock(Type,IEnumerable{Type},FakerMod)"/>
    Fake<T> Mock<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration);

    /// <inheritdoc cref="Mock(Type,IEnumerable{Type},FakerMod)"/>
    Fake Mock(Type parent, params IEnumerable<Type> interfaces);

    /// <summary>Creates a strict fake where calls fail unless set up.</summary>
    /// <param name="parent">Type being faked.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>Handler for fake behavior.</returns>
    Fake Mock(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration);

    /// /// <inheritdoc cref="Stub{T}(IEnumerable{Type},FakerMod)"/>
    Fake<T> Stub<T>(params IEnumerable<Type> interfaces);

    /// <typeparam name="T"><see cref="Type"/> being faked.</typeparam>
    /// <inheritdoc cref="Stub(Type,IEnumerable{Type},FakerMod)"/>
    Fake<T> Stub<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration);

    /// <inheritdoc cref="Stub(Type,IEnumerable{Type},FakerMod)"/>
    Fake Stub(Type parent, params IEnumerable<Type> interfaces);

    /// <summary>Creates a loose fake with a base default implementation.</summary>
    /// <param name="parent">Type being faked.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>Handler for the fake behavior.</returns>
    Fake Stub(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration);

    /// <inheritdoc cref="InjectMocks{T}(IEnumerable{object},FakerMod)"/>
    Injected<T> InjectMocks<T>(FakerMod? optionConfiguration = null);

    /// <summary>Creates an instance injected with mocks.</summary>
    /// <inheritdoc cref="InjectStubs{T}(IEnumerable{object},FakerMod)"/>
    Injected<T> InjectMocks<T>(IEnumerable<object> values, FakerMod? optionConfiguration = null);

    /// <inheritdoc cref="InjectStubs{T}(IEnumerable{object},FakerMod)"/>
    Injected<T> InjectStubs<T>(FakerMod? optionConfiguration = null);

    /// <summary>Creates an instance injected with stubs.</summary>
    /// <typeparam name="T">Instance type to be created.</typeparam>
    /// <param name="values">Values to inject instead where possible.</param>
    /// <param name="optionConfiguration">Modifications of Options to apply for this call.</param>
    /// <returns>The created instance with its fakes.</returns>
    Injected<T> InjectStubs<T>(IEnumerable<object> values, FakerMod? optionConfiguration = null);
}
