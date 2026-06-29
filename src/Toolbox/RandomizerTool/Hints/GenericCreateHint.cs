using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing generic types for <see cref="IRandomizer"/>.</summary>
public sealed class GenericCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.GenericHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (type?.IsGenericTypeDefinition ?? false)
        {
            foreach (Type defined in GenericResolver.CreateConcreteGenerics(type, randomizer))
            {
                try
                {
                    return new(
                        randomizer.CreateSpecific(
                            defined,
                            type,
                            opt => opt with { ContentRandomizationRequired = false }
                        )
                    );
                }
                catch (CreateAndFakeException)
                {
                    // Try next type.
                }
            }
            throw new UnsupportedException(
                $"Could not create generic '{GenericTypeConverter.ExpandedName(type)}'."
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
