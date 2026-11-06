namespace Werecodent.CreateAndFake.DuplicatorTool;

/// <summary>Enables deep copies with the aid of the duplicator.</summary>
/// <typeparam name="T">Self-reference <see cref="Type"/>.</typeparam>
/// <remarks>Should only be inherited by <see langword="sealed"/> <see cref="Type"/>s.</remarks>
public interface IDuplicatable<T>
    where T : IDuplicatable<T>
{
    /// <summary>
    ///     Makes a clone such that any mutation to the source
    ///     or copy only affects that object and not the other.
    /// </summary>
    /// <param name="duplicator">Duplicator to clone child values.</param>
    /// <returns>Created clone that is equal in value to the instance.</returns>
    T DeepClone(IDuplicator duplicator);
}
