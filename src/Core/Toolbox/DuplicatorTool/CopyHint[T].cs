namespace CreateAndFake.Toolbox.DuplicatorTool;

/// <typeparam name="T"><c>Type</c> being supported for cloning.</typeparam>
/// <inheritdoc/>
public abstract class CopyHint<T> : CopyHint
{
    /// <inheritdoc/>
    protected internal sealed override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
    {
        if (source is T data)
        {
            return new(Copy(data, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <summary>Deep clones <paramref name="source"/>.</summary>
    /// <param name="source">Object to clone.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    protected abstract T Copy(T source, DuplicatorChainer duplicator);
}
