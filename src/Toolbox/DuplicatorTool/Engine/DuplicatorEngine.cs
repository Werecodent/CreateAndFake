using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

#pragma warning disable RCS1165, S2955 // Checking for only null specifically.

/// <inheritdoc cref="IDuplicator"/>
public sealed class DuplicatorEngine : ToolEngine<ICopyHint>, IDuplicatorEngine
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(source))]
    public T Copy<T>(T source, IDuplicatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (source == null)
        {
            return default!;
        }

        CopyHintResult? result = SelectHints(chainer)
            .Select(h => h.TryCopy(source, chainer))
            .FirstOrDefault(r => r?.HasData ?? false);

        if (result != null)
        {
            return (T)result.Data!;
        }
        else
        {
            throw new UnsupportedException(
                $"Type '{GenericConverter.ExpandName(source)}' not supported by the duplicator. "
                    + "Create a hint to generate the type and pass it to the duplicator."
            );
        }
    }
}

#pragma warning restore
