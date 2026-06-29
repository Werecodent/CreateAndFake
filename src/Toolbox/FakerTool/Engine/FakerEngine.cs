using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool.Engine;

/// <inheritdoc cref="IFaker"/>
public sealed class FakerEngine : ToolEngine<IFakeHint>, IFakerEngine
{
    /// <inheritdoc/>
    public IFaked Fake(Type type, IFakerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        return Subclasser.Create(type, chainer.Options);
    }
}
