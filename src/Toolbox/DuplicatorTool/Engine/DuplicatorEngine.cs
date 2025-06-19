using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design;

namespace CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="IDuplicator"/>
/// <param name="defaultHints">Generators used to duplicate specific types.</param>
public sealed class DuplicatorEngine(ImmutableArray<CopyHint> defaultHints) : IDuplicatorEngine
{
    /// <summary>Picks hints to use for randomization based upon <paramref name="options"/>.</summary>
    /// <param name="options">Potentially modified configuration to use.</param>
    /// <returns>Cached hints if possible; built hints otherwise.</returns>
    private IEnumerable<CopyHint> SelectHints(DuplicatorOptions options)
    {
        foreach (CopyHint hint in options.Hints)
        {
            yield return hint;
        }
        if (options.IncludeDefaultHints)
        {
            foreach (CopyHint hint in defaultHints)
            {
                yield return hint;
            }
        }
    }

#pragma warning disable RCS1165, S2955 // Checking for only null.

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(source))]
    public T Copy<T>(T source, IDuplicatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer, nameof(chainer));

        if (source == null)
        {
            return default!;
        }
        CopyHintResult? result = SelectHints(chainer.Options)
            .Select(h => h.TryCopy(source, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return (T)result.Data!;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{source.GetType().FullName}' not supported by the duplicator. "
                    + "Create a hint to generate the type and pass it to the duplicator."
            );
        }
    }

#pragma warning restore RCS1165, S2955
}
