using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="AsyncHashSet{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class AsyncHashCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.AsyncHashHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(AsyncHashSet<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = GenericConverter.AsGenericBase(type);

        if (asGeneric == typeof(AsyncHashSet<>) || asGeneric == typeof(IAsyncSet<>))
        {
            Type contentType = type.GetGenericArguments().Single();

            dynamic backingData = randomizer.Create(
                typeof(List<>).MakeGenericType(contentType),
                _ => randomizer.Options
            );

            return new(
                AsyncHashSet.CreateFromAsync(
                    backingData,
                    (dynamic)
                        randomizer.Create(
                            typeof(IAsyncEqualityComparer<>).MakeGenericType(contentType)
                        ),
                    randomizer.Options.Valuer.Options.IterationLimit,
                    CancellationToken.None
                )
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
