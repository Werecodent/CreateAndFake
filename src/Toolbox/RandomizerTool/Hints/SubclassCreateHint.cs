using System.Diagnostics;
using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing objects in general for <see cref="IRandomizer"/>.</summary>
public sealed class SubclassCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.SubclassHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);
        if (randomizer.AlreadyCreated(type))
        {
            return CreateHintResult.None;
        }

        InheritanceTracker inheritance = InheritanceTracker.For(type);

        HashSet<Type> foundAlready = [];
        List<Type> subclasses =
        [
            .. randomizer
                .Options.Gen.NextSequence(
                    randomizer.Options.PreferLocalSubclasses
                        ? inheritance.FindLocalSubclasses().Concat([type])
                        : []
                )
                .Concat(
                    randomizer.Options.Gen.NextSequence(
                        inheritance.FindLoadedSubclasses().Concat([type])
                    )
                )
                .Where(t => !t.IsAbstract)
                .Where(foundAlready.Add)
                .Where(t => !randomizer.AlreadyCreated(t)),
        ];

        if (subclasses.Count == 1 && subclasses[0] == type)
        {
            return CreateHintResult.None;
        }

        foreach (Type subclass in subclasses)
        {
            try
            {
                return new(
                    randomizer.CreateSpecific(
                        subclass,
                        type,
                        opt => opt with { ContentRandomizationRequired = false }
                    )
                );
            }
            catch (CreateAndFakeException)
            {
                // Try next type.
                Debug.Write(subclass);
            }
        }

        return CreateHintResult.None;
    }
}
