using System.Text;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ExtractorTool.Engine;
using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

/// <summary>Combines and utilizes available handlers for mutations.</summary>
public sealed class HandlerExtractHint : IExtractHint
{
    /// <summary>Handlers to use that haven't already been specified.</summary>
    private static readonly IExtractHandler[] _Creators =
    [
        new SelfExtractHandler(typeof(Lock)),
        new SelfExtractHandler(typeof(string)),
new SelfExtractHandler(typeof(AssemblyName)),
        new SelfExtractHandler(typeof(StringBuilder)),
        new SelfExtractHandler(typeof(CancellationToken)),
        new SelfExtractHandler(typeof(RuntimeMethodHandle)),
        new ConvertExtractHandler(typeof(TypeDescriber), x => [((TypeDescriber)x).SupportedType]),
    ];

    /// <summary>All handlers by their supported type.</summary>
    private static readonly IDictionary<Type, IExtractHandler> _MutatorsByType =
        TypeSupporter.GroupBySupportedType(
            _Creators
                .Concat(ReflectionExtractHandlers.Handlers)
                .Concat(ValueExtractHandlers.Handlers)
        );

    /// <inheritdoc/>
    public int EnginePriority => (int)ExtractPriority.HandlerHint;

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _MutatorsByType.Keys;

    /// <inheritdoc/>
    public ExtractHintResult TryToExtract(object? source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (
            source != null
            && _MutatorsByType.TryGetValue(source.GetType(), out IExtractHandler? handler)
        )
        {
            return new(handler.ExtractSupported(source, chainer));
        }
        else
        {
            return ExtractHintResult.None;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
