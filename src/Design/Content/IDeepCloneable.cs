namespace CreateAndFake.Design.Content;

/// <summary>Provides self copy-by-value functionality.</summary>
public interface IDeepCloneable
{
    /// <summary>
    ///     Creates a clone where any mutation to <see langword="this"/> or the
    ///     created copy only affects that object and not the other.
    /// </summary>
    /// <returns>The created clone that is equal by value to <see langword="this"/>.</returns>
    IDeepCloneable DeepClone();
}
