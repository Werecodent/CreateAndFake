using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Handles randomizing specific types for <see cref="IRandomizer"/>.</summary>
public abstract class CreateHint : IToolHint
{
    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];

    /// <summary>Tries to create a random instance of the given <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to generate.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Possible result.</returns>
    public abstract CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
