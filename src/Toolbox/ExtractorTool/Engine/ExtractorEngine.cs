using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <inheritdoc cref="IExtractor"/>
public sealed class ExtractorEngine : ToolEngine<IExtractHint>, IExtractorEngine
{
    /// <inheritdoc/>
    public bool Extract(object? value, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (value == null)
        {
            return false;
        }

        ExtractHintResult? result = SelectHints(chainer)
            .Select(h => h.TryToExtract(value, chainer))
            .FirstOrDefault(r => r?.HasData ?? false);

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new UnsupportedException(
                $"Type '{GenericConverter.ExpandName(value)}' not supported by the extractor. "
                    + "Create a hint to extract the type."
            );
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExtractAsync(
        object? value,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);
        if (value == null)
        {
            return false;
        }

        ExtractHintResult? result = null;
        foreach (IExtractHint hint in SelectHints(chainer))
        {
            ExtractHintResult? current = await hint.TryToExtractAsync(value, chainer, canceler)
                .ConfigureAwait(false);

            if (current?.HasData ?? false)
            {
                result = current;
                break;
            }
        }

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new UnsupportedException(
                $"Type '{GenericConverter.ExpandName(value)}' not supported by the extractor. "
                    + "Create a hint to extract the type."
            );
        }
    }
}
