using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common <see cref="Exception"/> assertion calls.</summary>
/// <param name="error"><inheritdoc cref="Error" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertErrorBase<T>(IAsserter asserter, Exception? error)
    : AssertObjectBase<T>(asserter, error) where T : AssertErrorBase<T>
{
    /// <summary>Exception to run assertion checks with.</summary>
    protected Exception? Error { get; } = error;

    /// <inheritdoc/>
    [DoesNotReturn]
    public override void Fail(string? details = null)
    {
        Asserter.Fail(Error, details);
    }
}
