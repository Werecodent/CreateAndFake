namespace CreateAndFake.Design.Content;

/// <summary>Provides self copy-by-value functionality.</summary>
public interface IDeepCloneable
{
    /// <summary>
    ///     Creates a clone where any mutation to <c>this</c> or the
    ///     created copy only affects that object and not the other.
    /// </summary>
    /// <returns>The created clone that is equal by value to <c>this</c>.</returns>
    IDeepCloneable DeepClone();
}
