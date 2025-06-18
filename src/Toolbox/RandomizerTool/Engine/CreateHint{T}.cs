using CreateAndFake.Design;

namespace CreateAndFake.RandomizerTool.Engine;

/// <typeparam name="T"><see cref="Type"/> being supported for randomization.</typeparam>
/// <inheritdoc/>
public abstract class CreateHint<T> : CreateHint
{
    /// <inheritdoc/>
    public sealed override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (
            type.IsInheritedBy<T>()
            && (type != typeof(object) || typeof(T) == typeof(object))
            && !randomizer.AlreadyCreated<T>()
        )
        {
            return new(Create(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <summary>Creates a random <typeparamref name="T"/> instance.</summary>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>The created <typeparamref name="T"/> instance.</returns>
    protected abstract T Create(IRandomizerChainer randomizer);
}
