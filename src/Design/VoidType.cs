#pragma warning disable IDE0130

namespace Werecodent.CreateAndFake;

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

#pragma warning restore
