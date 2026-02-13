using System.Collections;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Design.Content;

/// <summary>Compares <see langword="object"/>s by reference.</summary>
/// <remarks>
///     Ignores overrides to <see cref="object.Equals(object)"/>
///     and <see cref="object.GetHashCode"/>.
/// </remarks>
public sealed class ReferenceComparer
    : IComparer,
        IComparer<object?>,
        IEqualityComparer,
        IEqualityComparer<object?>
{
    /// <summary>Default instance to use for comparing by reference.</summary>
    public static ReferenceComparer Use { get; } = new();

    /// <summary>
    ///     Determines if <paramref name="x"/> equals <paramref name="y"/> by reference.
    /// </summary>
    /// <param name="x">The <see langword="object"/> to compare with <paramref name="y"/>.</param>
    /// <param name="y">The <see langword="object"/> to compare with <paramref name="x"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="x"/> equals
    ///     <paramref name="y"/> by reference, <see langword="false"/> otherwise.
    /// </returns>
    public new bool Equals(object? x, object? y)
    {
        return ReferenceEquals(x, y);
    }

    /// <summary>
    ///     Computes an identifying hash code for <paramref name="obj"/> based upon reference.
    /// </summary>
    /// <param name="obj">The <see langword="object"/> to generate a hash code for.</param>
    /// <returns>The reference computed hash code for <paramref name="obj"/>.</returns>
    public int GetHashCode(object? obj)
    {
        return RuntimeHelpers.GetHashCode(obj);
    }

    /// <summary>
    ///     Compares <paramref name="x"/> and <paramref name="y"/>
    ///     by their reference hash for sorting.
    /// </summary>
    /// <param name="x">The <see langword="object"/> to compare with <paramref name="y"/>.</param>
    /// <param name="y">The <see langword="object"/> to compare with <paramref name="x"/>.</param>
    /// <returns><list type="bullet">
    ///     <item>Positive value if <paramref name="x"/> &gt; <paramref name="y"/>.</item>
    ///     <item>Zero if <paramref name="x"/> = <paramref name="y"/>.</item>
    ///     <item>Negative value if <paramref name="x"/> &lt; <paramref name="y"/>.</item>
    /// </list></returns>
    public int Compare(object? x, object? y)
    {
        return ReferenceEquals(x, y) ? 0 : GetHashCode(x).CompareTo(GetHashCode(y));
    }
}
