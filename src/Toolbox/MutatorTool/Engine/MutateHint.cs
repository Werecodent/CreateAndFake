using CreateAndFake.Design.Types;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc cref="IMutateHint"/>
public abstract class MutateHint : IMutateHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> SupportedTypes { get; }

    /// <inheritdoc/>
    public MutateHintResult TryToModify(object instance, IMutatorChainer chainer)
    {
        return Supports(instance) ? new(Modify(instance, chainer)) : MutateHintResult.None;
    }

    /// <summary>If the hint supports modifying the <paramref name="instance"/>.</summary>
    /// <returns><see langword="true"/> if supported, <see langword="false"/> otherwise.</returns>
    /// <inheritdoc cref="TryToModify"/>
    protected abstract bool Supports(object instance);

    /// <returns>
    ///     <see langword="true"/> if the <paramref name="instance"/> has been mutated,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    /// <inheritdoc cref="TryToModify"/>
    protected abstract bool Modify(object instance, IMutatorChainer chainer);

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericTypeConverter.ExpandedName(GetType());
    }
}
