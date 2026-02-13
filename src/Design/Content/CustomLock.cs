namespace CreateAndFake.Design.Content;

/// <summary>
///     Type for <see langword="lock"/>s to prevent
///     <see cref="object"/> <see cref="Type"/> conflicts.
/// </summary>
/// <param name="id"><inheritdoc cref="Id" path="/summary"/></param>
/// <remarks>System.Threading.Lock should be used past .NET 9.</remarks>
public sealed class CustomLock(Guid? id = null)
{
    /// <summary>Lock identifier.</summary>
    public Guid Id { get; } = id ?? Guid.NewGuid();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Id == (obj as CustomLock)?.Id;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
