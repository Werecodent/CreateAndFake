using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.MutatorTool.Engine;

/// <summary>Handles mutation of the <see cref="ITypeSupporter.SupportedType"/>.</summary>
public interface IMutateHandler : ITypeSupporter
{
    /// <inheritdoc cref="MutateHint.Modify(object, IMutatorChainer)"/>
    bool ModifySupported(object instance, IMutatorChainer chainer);
}
