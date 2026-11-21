using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

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
            int i = 0;
            foreach (Type defined in GenericResolver.CreateConcreteGenerics(type, randomizer))
            {
                ArgumentGuard.ThrowUponIterationLimit(
                    i++,
                    randomizer.Options.Design.IterationLimit
                );
                try
                {
                    return new(
                        randomizer.CreateSpecific(defined, type, opt =>
                            opt with
                            {
                                RequireRandomizedContents = false,
                            }
                        )
                    );
                }
                catch (CreateAndFakeException)
                {
                    // Try next type.
                }
            }
            throw new UnsupportedException(
                $"Could not create generic '{GenericConverter.ExpandName(type)}'."
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
