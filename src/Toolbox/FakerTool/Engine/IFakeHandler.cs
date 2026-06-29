using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool.Engine;

/// <summary>Handles faking the <see cref="ITypeSupporter.SupportedType"/>.</summary>
internal interface IFakeHandler : ITypeSupporter
{
    /// <summary>Fakes an instance of the specific type.</summary>
    /// <param name="source">Object to fake.</param>
    /// <param name="faker">Handles faking child values.</param>
    /// <returns>The clone.</returns>
    IFaked FakeSupported(object source, IFakerChainer faker);
}
