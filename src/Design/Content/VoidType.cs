namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Represents <see langword="void"/>.</summary>
public sealed class VoidType
{
    /// <summary>Singleton instance to use.</summary>
    public static VoidType Instance { get; } = new VoidType();

    /// <inheritdoc cref="VoidType"/>
    private VoidType() { }

    /// <inheritdoc/>
    public override string ToString()
    {
        return "'void'";
    }
}
