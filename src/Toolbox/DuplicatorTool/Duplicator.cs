using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool;

/// <inheritdoc cref="IDuplicator"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/> </param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Duplicator(DuplicatorOptions options) : IDuplicator
{
    /// <summary>Handles hint based duplication.</summary>
    private static readonly DuplicatorEngine _Engine = new();

    /// <inheritdoc/>
    public DuplicatorOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _Engine.SupportedTypes;

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(source))]
    public T Copy<T>(T source, DuplicatorMod? optionConfiguration = null)
    {
        DuplicatorOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;
        try
        {
            T result = new DuplicatorChainer(localOptions, _Engine).Copy(source);
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
                $"Issue duplicating type '{GenericConverter.ExpandName(source)}'.",
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
