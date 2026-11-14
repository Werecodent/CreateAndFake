using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing async collection instances for <see cref="IRandomizer"/>.</summary>
public sealed class AsyncCollectionCreateHint : CreateHint
{
    /// <summary>Method used to create collection contents.</summary>
    private static readonly MethodInfo _TaskMaker = typeof(AsyncCollectionCreateHint).GetMethod(
        nameof(CreateContentItem),
        BindingFlags.Static | BindingFlags.NonPublic
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.AsyncCollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [
            typeof(AsyncHashSet<>),
            typeof(IAsyncSet<>),
            typeof(AsyncList<>),
            typeof(IAsyncEnumerable<>),
        ];

    private static T CreateContentItem<T>(IRandomizerChainer randomizer)
    {
        return randomizer.Create<T>();
    }

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = GenericConverter.AsGenericBase(type);

        if (asGeneric == typeof(AsyncHashSet<>) || asGeneric == typeof(IAsyncSet<>))
        {
            Type contentType = type.GetGenericArguments().Single();

            MethodInfo genericMaker = _TaskMaker.MakeGenericMethod(contentType);
            object contentFactory()
            {
                return genericMaker.Invoke(null, [randomizer])!;
            }

            return new(
                AsyncHashSet.CreateViaGeneratorAsync(
                    randomizer.Options.NextCollectionSize(),
                    (Func<object>)contentFactory,
                    (dynamic)
                        randomizer.Create(
                            typeof(IAsyncEqualityComparer<>).MakeGenericType(contentType)
                        ),
                    randomizer.Options.Design.IterationLimit,
                    CancellationToken.None
                )
            );
        }
        else if (asGeneric == typeof(AsyncList<>) || asGeneric == typeof(IAsyncEnumerable<>))
        {
            Type contentType = type.GetGenericArguments().Single();

            dynamic backingData = randomizer.Create(
                typeof(List<>).MakeGenericType(contentType),
                _ => randomizer.Options
            );

            return new(AsyncSeriesHelper.CreateFromAsync(backingData, int.MaxValue));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
