using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.DuplicatorTool.Engine;

#pragma warning disable RCS1165, S2955 // Checking for only null specifically.

/// <inheritdoc cref="IDuplicator"/>
public sealed class DuplicatorEngine : ToolEngine<CopyHint>, IDuplicatorEngine
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
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return (T)result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{TypeDescriber.ExpandedName(source.GetType())}' "
                    + "not supported by the duplicator. Create a hint to "
                    + "generate the type and pass it to the duplicator."
            );
        }
    }
}

#pragma warning restore RCS1165, S2955
