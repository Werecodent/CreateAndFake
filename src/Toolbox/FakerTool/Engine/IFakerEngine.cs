using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool.Engine;

/// <inheritdoc cref="IFaker"/>
public interface IFakerEngine : IToolEngine<IFakeHint>
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IFaker.Stub(Type,IEnumerable{Type},FakerMod)"/>
    IFaked Fake(Type type, IFakerChainer chainer);
}
