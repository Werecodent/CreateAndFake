using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing instances utilizing <see cref="ICompareHandler"/>s for <see cref="IValuer"/>.</summary>
public sealed class HandlerCompareHint : CompareHint
{
    /// <summary>Supported types and the methods used to compare them.</summary>
    private static readonly ICompareHandler[] _Handlers = [];

    private static readonly IDictionary<Type, ICompareHandler> _HandlersByType =
        TypeSupporter.GroupBySupportedType(_Handlers.Concat(ReflectionCompareHandlers.Handlers));

    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        return expected != null
            && _HandlersByType.TryGetValue(expected.GetType(), out ICompareHandler? comparer)
            && comparer.SupportedType == actual?.GetType();
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, actual, valuer);

        return _HandlersByType[expected.GetType()].CompareSupported(expected, actual, valuer);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, valuer);

        return _HandlersByType[item.GetType()].HashSupported(item, valuer);
    }
}
