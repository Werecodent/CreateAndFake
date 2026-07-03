using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsserterTool;

/// <inheritdoc cref="IAsserter"/>
public partial class Asserter : IAsserterError
{
    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public virtual void Fail(Exception? exception, string? details = null)
    {
        Fail(exception, Unconfigured, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public virtual void Fail(
        Exception? exception,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        throw new AssertException("Test failed.", details, localOptions.Gen.InitialSeed, exception);
    }

    /// <inheritdoc/>
    public void Debug(Exception? exception, string? details = null)
    {
        Debug(exception, Unconfigured, details);
    }

    /// <inheritdoc/>
    public void Debug(
        Exception? exception,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        AsserterOptions localOptions = ApplyConfiguration(optionConfiguration);
        if (localOptions.DebugAssertsFail)
        {
            throw new AssertException(
                $"{nameof(AsserterOptions.DebugAssertsFail)} set to '{true}'.",
                details,
                localOptions.Gen.InitialSeed,
                exception
            );
        }
    }
}
