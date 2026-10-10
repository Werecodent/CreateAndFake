namespace Werecodent.CreateAndFake.MutatorTool.Engine;

/// <summary>Handles <typeparamref name="T"/> mutations.</summary>
/// <typeparam name="T">Specific <see cref="Type"/> the hint supports.</typeparam>
public abstract class MutateHint<T> : MutateHint
{
    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes { get; } = [typeof(T)];

    /// <inheritdoc/>
    protected override bool Supports(object instance)
    {
        return instance is T;
    }

    /// <inheritdoc/>
    protected sealed override bool Modify(object instance, IMutatorChainer chainer)
    {
        return Modify((T)instance, chainer);
    }

    /// <inheritdoc cref="Modify(object,IMutatorChainer)"/>
    protected abstract bool Modify(T instance, IMutatorChainer chainer);
}
