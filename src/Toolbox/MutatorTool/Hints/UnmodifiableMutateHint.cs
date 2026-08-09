using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class UnmodifiableMutateHint : MutateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.UnmodifiableHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(IAsyncEnumerable<>)];

    /// <inheritdoc/>
    protected override bool Supports(object instance)
    {
        ArgumentGuard.ThrowIfNull(instance);

        Type type = instance.GetType();
        return type.IsEnum || type.Inherits(typeof(IAsyncEnumerable<>));
    }

    /// <inheritdoc/>
    protected override bool Modify(object instance, IMutatorChainer chainer)
    {
        return false;
    }
}
