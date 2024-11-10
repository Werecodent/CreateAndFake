namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common <see cref="Exception"/> assertion calls.</summary>
/// <param name="error"><inheritdoc cref="Error" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertErrorBase<T>(AsserterOptions options, Exception? error)
    : AssertObjectBase<T>(options, error) where T : AssertErrorBase<T>
{
    /// <summary>Exception to run assertion checks with.</summary>
    protected Exception? Error { get; } = error;

    /// <inheritdoc/>
    public override void Fail(string? details = null)
    {
        if (Error != null)
        {
            throw new AssertException("Test failed.", details, Options.Gen.InitialSeed, Error);
        }
        else
        {
            throw new AssertException("Test failed.", details, Options.Gen.InitialSeed);
        }
    }
}
