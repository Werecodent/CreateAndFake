using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing comparer instances for <see cref="IRandomizer"/>.</summary>
public sealed class ComparerCreateHint : CreateHint
{
    /// <summary>Creator for <see cref="IAsyncEqualityComparer{T}"/> instances.</summary>
    private static readonly MethodInfo _AsyncComparerMaker = typeof(IValuer).GetMethod(
        nameof(IValuer.ToAsyncComparer)
    )!;

    /// <summary>Creator for <see cref="IEqualityComparer{T}"/> instances.</summary>
    private static readonly MethodInfo _ComparerMaker = typeof(IValuer).GetMethod(
        nameof(IValuer.ToComparer)
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.ComparerHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(IAsyncEqualityComparer<>), typeof(IEqualityComparer<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = GenericConverter.AsGenericBase(type);

        if (asGeneric == typeof(IAsyncEqualityComparer<>))
        {
            return new(
                _AsyncComparerMaker
                    .MakeGenericMethod(type.GetGenericArguments().Single())
                    .Invoke(randomizer.Options.Valuer, [])
            );
        }
        else if (asGeneric == typeof(IEqualityComparer<>))
        {
            return new(
                _ComparerMaker
                    .MakeGenericMethod(type.GetGenericArguments().Single())
                    .Invoke(randomizer.Options.Valuer, [])
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
