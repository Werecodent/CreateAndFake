namespace Werecodent.CreateAndFake.Design.Comparisons;

/// <summary>Provides self copy-by-value functionality.</summary>
/// <typeparam name="T">Self-reference <see cref="Type"/>.</typeparam>
/// <remarks>Should only be inherited by <see langword="sealed"/> <see cref="Type"/>s.</remarks>
public interface IDeepCloneable<T>
    where T : IDeepCloneable<T>
{
    /// <summary>
    ///     Creates a clone where any mutation to <see langword="this"/>
    ///     or the returned copy only alters that object and not the other.
    /// </summary>
    /// <returns>The created clone that is equal by value to <see langword="this"/>.</returns>
    T DeepClone();
}
