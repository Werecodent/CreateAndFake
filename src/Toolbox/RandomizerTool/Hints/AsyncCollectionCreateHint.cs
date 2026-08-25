using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IRandomizer"/>.</summary>
public sealed class AsyncCollectionCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.AsyncCollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(IAsyncEnumerable<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? genericType = GenericConverter.AsConcreteType(type, typeof(IAsyncEnumerable<>));
        if (genericType != null)
        {
            Type itemType = genericType.GetGenericArguments().Single();

            Type asyncList = typeof(AsyncList<>).MakeGenericType(itemType);
            if (type.IsInheritedBy(asyncList))
            {
                object backingData = randomizer.Create(
                    typeof(List<>).MakeGenericType(itemType),
                    _ => randomizer.Options
                );

                return new(Activator.CreateInstance(asyncList, backingData));
            }
        }

        return CreateHintResult.None;
    }
}
