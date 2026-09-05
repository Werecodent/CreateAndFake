using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="AsyncHashSet{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class AsyncHashCreateHint : CreateHint
{
    /// <summary>Creator for <see cref="IAsyncEqualityComparer{T}"/> instances.</summary>
    private static readonly MethodInfo _ComparerMaker = typeof(IValuer).GetMethod(
        nameof(IValuer.ToAsyncComparer)
    )!;

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

            object backingData = randomizer.Create(
                typeof(List<>).MakeGenericType(contentType),
                _ => randomizer.Options
            );

            MethodInfo maker = TypeDescriber
                .For(typeof(AsyncHashSet<>).MakeGenericType(contentType))
                .Factories.OnlyPublic.Single(m =>
                    m.GetParameters()[0].ParameterType
                    == typeof(IEnumerable<>).MakeGenericType(contentType)
                );

            return new(
                maker.Invoke(
                    null,
                    [
                        backingData,
                        _ComparerMaker
                            .MakeGenericMethod(contentType)
                            .Invoke(randomizer.Options.Valuer, []),
                        randomizer.Options.Valuer.Options.IterationLimit,
                        CancellationToken.None,
                    ]
                )
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
