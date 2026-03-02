using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.Design.Types;

namespace CreateAndFake.DuplicatorTool.Engine;

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
                $"Type '{TypeDescriber.ExpandedName(source)}' not supported by the duplicator. "
                    + "Create a hint to generate the type and pass it to the duplicator."
            );
        }
    }
}

#pragma warning restore RCS1165, S2955
