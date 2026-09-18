using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.AsserterTool.Categories;

namespace Werecodent.CreateAndFake.AsserterTool;

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
    public TException HasInner<TException>(Exception? exception, string? details = null)
        where TException : Exception
    {
        return HasInner<TException>(exception, Unconfigured, details);
    }

    /// <inheritdoc/>
    public TException HasInner<TException>(
        Exception? exception,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        Exception? inner = exception?.InnerException;
        IsNotNull(inner, optionConfiguration, details);
        Inherits<TException>(inner.GetType(), details);
        return (TException)inner;
    }

    /// <inheritdoc/>
    public void HasInnerException(Exception? outer, Exception? inner, string? details = null)
    {
        HasInnerException(outer, inner, Unconfigured, details);
    }

    /// <inheritdoc/>
    public void HasInnerException(
        Exception? outer,
        Exception? inner,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Is(inner, outer?.InnerException, optionConfiguration, details);
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
