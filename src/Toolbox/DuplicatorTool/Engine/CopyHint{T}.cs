namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

/// <typeparam name="T"><see cref="Type"/> being supported for cloning.</typeparam>
/// <inheritdoc/>
public abstract class CopyHint<T> : CopyHint
{
    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(T)];

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
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
    protected abstract T Copy(T source, IDuplicatorChainer duplicator);
}
