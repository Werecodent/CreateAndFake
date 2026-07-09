using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles common <see cref="Exception"/> assertion calls.</summary>
/// <param name="error"><inheritdoc cref="Error" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertErrorBase<T>(IAsserter asserter, Exception? error)
    : AssertObjectBase<T>(asserter, error)
    where T : AssertErrorBase<T>
{
    /// <summary>Exception to run assertion checks with.</summary>
    protected Exception? Error { get; } = error;

    /// <inheritdoc cref="IAsserterError.Fail(Exception,string)"/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public override void Fail(string? details = null)
    {
        Asserter.Fail(Error, details);
    }

    /// <inheritdoc cref="IAsserterError.Fail(Exception,AsserterMod,string)"/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public override void Fail(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.Fail(Error, optionConfiguration, details);
    }

    /// <inheritdoc cref="IAsserterError.HasInner{T}(Exception,string)"/>
    public ExceptionChainer<TException> HasInner<TException>(string? details = null)
        where TException : Exception
    {
        return new(Asserter.HasInner<TException>(Error, details), Asserter);
    }

    /// <inheritdoc cref="IAsserterError.HasInner{T}(Exception,AsserterMod,string)"/>
    public ExceptionChainer<TException> HasInner<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return new(Asserter.HasInner<TException>(Error, optionConfiguration, details), Asserter);
    }

    /// <inheritdoc cref="IAsserterError.HasInnerException(Exception,Exception,string)"/>
    public AssertChainer<T> HasInnerException(Exception? inner, string? details = null)
    {
        Asserter.HasInnerException(Error, inner, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterError.HasInnerException(Exception,Exception,AsserterMod,string)"/>
    public AssertChainer<T> HasInnerException(
        Exception? inner,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.HasInnerException(Error, inner, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterError.Debug(Exception,string)"/>
    public override AssertChainer<T> Debug(string? details = null)
    {
        Asserter.Debug(Error, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterError.Debug(Exception,AsserterMod,string)"/>
    public override AssertChainer<T> Debug(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.Debug(Error, optionConfiguration, details);
        return ToChainer();
    }
}
