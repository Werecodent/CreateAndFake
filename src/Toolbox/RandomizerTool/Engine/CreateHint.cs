using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="ICreateHint"/>
public abstract class CreateHint : ICreateHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<Type> SupportedTypes { get; }

    /// <inheritdoc/>
    public abstract CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer);

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
