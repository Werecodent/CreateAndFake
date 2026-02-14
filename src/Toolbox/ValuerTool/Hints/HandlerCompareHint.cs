using System.Collections;
using System.Collections.Specialized;
using System.Text;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing instances utilizing <see cref="ICompareHandler"/>s for <see cref="IValuer"/>.</summary>
public sealed class HandlerCompareHint : CompareHint
{
    /// <summary>Supported types and the methods used to compare them.</summary>
    private static readonly ICompareHandler[] _Handlers =
    [
        new DefaultEqualityCompareHandler(typeof(string)),
        new ConvertCompareHandler<StringBuilder>((s, _) => s.ToString()),
        new ConvertCompareHandler<StringDictionary>(
            (dict, _) =>
                dict.Cast<DictionaryEntry>().ToDictionary(e => (string)e.Key, e => (string?)e.Value)
        ),
        new ConvertCompareHandler<SeededRandom>(
            (r, c) =>
                c.Options.IgnoreCurrentRandomSeed ? r.InitialSeed : new[] { r.InitialSeed, r.Seed }
        ),
    ];

    private static readonly IDictionary<Type, ICompareHandler> _HandlersByType =
        TypeSupporter.GroupBySupportedType(_Handlers.Concat(ReflectionCompareHandlers.Handlers));

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.HandlerHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _HandlersByType.Keys;

    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        return expected != null
            && expected.GetType() == actual?.GetType()
            && _HandlersByType.ContainsKey(expected.GetType());
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
