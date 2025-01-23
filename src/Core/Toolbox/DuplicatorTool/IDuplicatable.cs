namespace CreateAndFake.Toolbox.DuplicatorTool;

/// <summary>Enables deep copies with the aid of the duplicator.</summary>
public interface IDuplicatable
{
    /// <summary>
    ///     Makes a clone such that any mutation to the source
    ///     or copy only affects that object and not the other.
    /// </summary>
    /// <param name="duplicator">Duplicator to clone child values.</param>
    /// <returns>Created clone that is equal in value to the instance.</returns>
    IDuplicatable DeepClone(IDuplicator duplicator);
}
