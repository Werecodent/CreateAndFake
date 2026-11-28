using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.MutatorTool.Engine;

/// <summary>Handles mutation of a concrete <see cref="Type"/>.</summary>
/// <remarks>For optimized performance by keying off the <see cref="ITypeSupporter.SupportedType"/>.</remarks>
public interface IMutateHandler : ITypeSupporter
{
    /// <inheritdoc cref="MutateHint.Modify(object, IMutatorChainer)"/>
    bool ModifySupported(object instance, IMutatorChainer chainer);
}
