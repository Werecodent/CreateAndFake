using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing instances utilizing <see cref="ICompareHandler"/>s for <see cref="IValuer"/>.</summary>
public sealed class HandlerCompareHint : CompareHint
{
    /// <summary>Supported types and the methods used to compare them.</summary>
    /// <remarks>WARNING: Not async compatible. Internals must have no async content.</remarks>
    private static readonly ICompareHandler[] _Handlers =
    [
        new ConvertCompareHandler<SeededRandom>(
            (rand, c) =>
                c.Options.IgnoreCurrentRandomSeed
                    ? rand.InitialSeed
                    : new[] { rand.InitialSeed, rand.Seed }
        ),
    ];

    private static readonly IDictionary<Type, ICompareHandler> _HandlersByType =
        TypeSupporter.GroupBySupportedType(
            _Handlers
                .Concat(ValueCompareHandlers.Handlers)
                .Concat(SystemCompareHandlers.Handlers)
                .Concat(ReflectionCompareHandlers.Handlers)
        );

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.HandlerHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _HandlersByType.Keys;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        Type type = expected.GetType();
        return _HandlersByType.ContainsKey(type) && type == actual.GetType();
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        return _HandlersByType[expected.GetType()].CompareSupported(expected, actual, chainer);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        return _HandlersByType[item.GetType()].HashSupported(item, chainer);
    }
}
