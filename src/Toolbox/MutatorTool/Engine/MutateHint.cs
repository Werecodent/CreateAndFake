using CreateAndFake.Design.Content;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc cref="IMutateHint"/>
public abstract class MutateHint : IMutateHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> SupportedTypes { get; }

    /// <inheritdoc/>
    public MutateHintResult TryModifying(object instance, IMutatorChainer chainer)
    {
        return Supports(instance) ? new(Modify(instance, chainer)) : MutateHintResult.None;
    }

    /// <summary>If the hint supports modifying the <paramref name="instance"/>.</summary>
    /// <param name="instance">Instance to modify.</param>
    /// <returns><see langword="true"/> if supported, <see langword="false"/> otherwise.</returns>
    protected abstract bool Supports(object instance);

    /// <inheritdoc cref="TryModifying"/>
    protected abstract bool Modify(object instance, IMutatorChainer chainer);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
