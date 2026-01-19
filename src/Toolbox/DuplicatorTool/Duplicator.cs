using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Hints;

namespace CreateAndFake.DuplicatorTool;

/// <inheritdoc cref="IDuplicator"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/> </param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Duplicator(DuplicatorOptions options) : IDuplicator
{
    /// <summary>Default set of hints to use for copying.</summary>
    internal static readonly ImmutableArray<CopyHint> DefaultHints =
    [
        new CopierCopyHint(),
        new TaskCopyHint(),
        new DeepCloneableCopyHint(),
        new DuplicatableCopyHint(),
        new BasicCopyHint(),
        new AsyncCollectionCopyHint(),
        new FrozenCollectionCopyHint(),
        new ImmutableCollectionCopyHint(),
        new LegacyCollectionCopyHint(),
        new CollectionCopyHint(),
        new TypeInfoCopyHint(),
        new CloneableCopyHint(),
        new SerializableCopyHint(),
        new ObjectCopyHint(),
    ];

    /// <summary>Handles hint based duplication.</summary>
    private static readonly DuplicatorEngine _engine = new(DefaultHints);

    /// <inheritdoc/>
    public DuplicatorOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _engine.SupportedTypes;

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(source))]
    public T Copy<T>(T source, DuplicatorMod? optionConfiguration = null)
    {
        DuplicatorOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        try
        {
            T result = new DuplicatorChainer(localOptions, _engine).Copy(source);
            if (localOptions.VerifyCloneResult)
            {
                Options.Asserter.ValuesEqual(
                    source,
                    result,
                    opt =>
                        opt with
                        {
                            Valuer = opt.Valuer.WithOptions(valuer =>
                                valuer with
                                {
                                    SkipAsyncValues = true,
                                }
                            ),
                        },
                    $"Type '{source?.GetType()}' did not clone properly. "
                        + "Verify/create a hint to generate the type and pass it to the duplicator."
                );
            }
            return result;
        }
        catch (Exception e)
        {
            throw new ToolException(
                $"Issue duplicating type '{TypeDescriber.ExpandedName(source?.GetType())}'.",
                e
            );
        }
    }

    /// <inheritdoc/>
    public IDuplicator WithOptions(DuplicatorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Duplicator(optionConfiguration.Invoke(Options));
    }
}
