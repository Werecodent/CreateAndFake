using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.CopyHints;

namespace CreateAndFake.DuplicatorTool;

/// <inheritdoc cref="IDuplicator"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/> </param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public sealed class Duplicator(DuplicatorOptions options) : IDuplicator
{
    /// <summary>Default set of hints to use for copying.</summary>
    private static readonly ImmutableArray<CopyHint> _DefaultHints =
    [
        new CommonSystemCopyHint(),
        new TaskCopyHint(),
        new DeepCloneableCopyHint(),
        new DuplicatableCopyHint(),
        new BasicCopyHint(),
        new AsyncCollectionCopyHint(),
        new FrozenCollectionCopyHint(),
        new ImmutableCollectionCopyHint(),
        new LegacyCollectionCopyHint(),
        new CollectionCopyHint(),
        new CloneableCopyHint(),
        new SerializableCopyHint(),
        new ObjectCopyHint(),
    ];

    /// <inheritdoc/>
    public DuplicatorOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Generators used to copy specific types.</summary>
    private readonly ImmutableArray<CopyHint> _hints = BuildHints(options);

    /// <summary>Builds hints to use for randomization based upon <paramref name="newOptions"/>.</summary>
    /// <param name="newOptions">Configuration for randomization.</param>
    /// <returns>Built hints to use.</returns>
    private static ImmutableArray<CopyHint> BuildHints(DuplicatorOptions newOptions)
    {
        return newOptions.IncludeDefaultHints
            ? newOptions.Hints.AddRange(_DefaultHints)
            : newOptions.Hints;
    }

    /// <summary>Picks hints to use for randomization based upon <paramref name="localOptions"/>.</summary>
    /// <param name="localOptions">Potentially modified configuration to use.</param>
    /// <returns>Cached hints if possible; built hints otherwise.</returns>
    private ImmutableArray<CopyHint> SelectHints(DuplicatorOptions localOptions)
    {
        return
            Options.IncludeDefaultHints == localOptions.IncludeDefaultHints
            && Options.Hints == localOptions.Hints
            ? _hints
            : BuildHints(localOptions);
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(source))]
    public T Copy<T>(T source, DuplicatorMod? optionConfiguration = null)
    {
        DuplicatorOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        try
        {
            T result = Copy(source, new DuplicatorChainer(localOptions, this, Copy));
            if (
                localOptions.VerifyCloneResult
                && !(source?.GetType()).Inherits(typeof(IAsyncEnumerable<>))
            )
            {
                try
                {
                    Options.Asserter.ValuesEqual(
                        source,
                        result,
                        $"Type '{source?.GetType()}' did not clone properly. "
                            + "Verify/create a hint to generate the type and pass it to the duplicator."
                    );
                }
                catch (ToolException)
                {
                    // Verification is not required and containing IAsyncEnumerable throws here.
                }
            }
            return result;
        }
        catch (InsufficientExecutionStackException e)
        {
            throw new InsufficientExecutionStackException(
                $"Ran into infinite generation trying to duplicate type '{source!.GetType().Name}'.",
                e
            );
        }
    }

    /// <param name="chainer">Handles cloning child values.</param>
    /// <inheritdoc cref="Copy{T}(T,DuplicatorMod)"/>
    [return: NotNullIfNotNull(nameof(source))]
    private T Copy<T>(T source, DuplicatorChainer chainer)
    {
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
}
