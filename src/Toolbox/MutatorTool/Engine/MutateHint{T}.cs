namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc/>
public abstract class MutateHint<T> : MutateHint
{
    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes { get; } = [typeof(T)];

    /// <inheritdoc/>
    protected override bool Supports(object instance)
    {
        return instance is T;
    }
}
