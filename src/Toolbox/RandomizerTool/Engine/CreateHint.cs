using CreateAndFake.Design.Types;

namespace CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="ICreateHint"/>
public abstract class CreateHint : ICreateHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> SupportedTypes { get; }

    /// <inheritdoc/>
    public abstract CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
