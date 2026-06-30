using CreateAndFake.Design.Tooling;

namespace CreateAndFake.FakerTool.Engine;

/// <inheritdoc cref="IFaker"/>
public interface IFakerEngine : IToolEngine<IFakeHint>
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IFaker.Supports(Type,FakerMod)"/>
    bool Supports(Type type, IFakerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IFaker.Mock(Type,IEnumerable{Type},FakerMod)"/>
    Fake Mock(Type parent, IEnumerable<Type> interfaces, IFakerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IFaker.Stub(Type,IEnumerable{Type},FakerMod)"/>
    Fake Stub(Type parent, IEnumerable<Type> interfaces, IFakerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IFaker.InjectMocks{T}(IEnumerable{object},FakerMod)"/>
    Injected<T> InjectMocks<T>(IEnumerable<object> values, IFakerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IFaker.InjectStubs{T}(IEnumerable{object},FakerMod)"/>
    Injected<T> InjectStubs<T>(IEnumerable<object> values, IFakerChainer chainer);
}
