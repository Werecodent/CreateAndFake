using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool;
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

    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public override void Fail(string? details = null)
    {
        Asserter.Fail(Error, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn, ExcludeFromCodeCoverage]
    public override void Fail(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.Fail(Error, optionConfiguration, details);
    }

    /// <inheritdoc/>
    public override AssertChainer<T> Debug(string? details = null)
    {
        Asserter.Debug(Error, details);
        return ToChainer();
    }

    /// <inheritdoc/>
    public override AssertChainer<T> Debug(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.Debug(Error, optionConfiguration, details);
        return ToChainer();
    }
}
