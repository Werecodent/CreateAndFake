using System.Collections;
using System.Collections.Specialized;
using System.Text;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool.Engine;
using CreateAndFake.ValuerTool.Handlers;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing instances utilizing <see cref="ICompareHandler"/>s for <see cref="IValuer"/>.</summary>
public sealed class HandlerCompareHint : CompareHint
{
    /// <summary>Supported types and the methods used to compare them.</summary>
    /// <remarks>WARNING: Not async compatible. Internals must have no async content.</remarks>
    private static readonly ICompareHandler[] _Handlers =
    [
        new DefaultEqualityCompareHandler(typeof(string)),
        new DefaultEqualityCompareHandler(typeof(RuntimeMethodHandle)),
        new ConvertCompareHandler<StringBuilder>((s, _) => s.ToString()),
        new ConvertCompareHandler<UriBuilder>((s, _) => s.Uri),
        new ConvertCompareHandler<CancellationToken>((token, _) => token.IsCancellationRequested),
        new ConvertCompareHandler<StringDictionary>(
            (dict, _) =>
                dict.Cast<DictionaryEntry>().ToDictionary(e => (string)e.Key, e => (string?)e.Value)
        ),
        new ConvertCompareHandler<SeededRandom>(
            (rand, c) =>
                c.Options.IgnoreCurrentRandomSeed
                    ? rand.InitialSeed
                    : new[] { rand.InitialSeed, rand.Seed }
        ),
        new ConvertCompareHandler<ValueTask>(
            (task, _) =>
                new
                {
                    SingleCallSource = SingleCallValueTaskSource.ExtractFrom(task),
                    Token = SingleCallValueTaskSource.ExtractTokenFrom(task),
                }
        ),
#if NET9_0_OR_GREATER
        new DefaultEqualityCompareHandler(typeof(System.Threading.Lock)),
#endif
    ];

    private static readonly IDictionary<Type, ICompareHandler> _HandlersByType =
        TypeSupporter.GroupBySupportedType(
            _Handlers
                .Concat(ValueCompareHandlers.Handlers)
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
