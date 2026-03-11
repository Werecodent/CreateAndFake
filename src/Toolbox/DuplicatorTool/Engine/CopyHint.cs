using CreateAndFake.Design.Types;

namespace CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="ICopyHint"/>
public abstract class CopyHint : ICopyHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> SupportedTypes { get; }

    /// <inheritdoc/>
    public abstract CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeHelper.ExpandedName(GetType());
    }
}
