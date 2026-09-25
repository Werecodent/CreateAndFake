namespace Werecodent.CreateAndFake.Design.Comparisons;

/// <summary>Checks if two objects are equal with async compatibility.</summary>
/// <typeparam name="T">Supported type for comparisons.</typeparam>
public interface IAsyncEqualityComparer<T>
{
    /// <summary>Determines if <paramref name="x"/> equals <paramref name="y"/> by value.</summary>
    /// <param name="x">Object to compare with <paramref name="y"/>.</param>
    /// <param name="y">Object to compare with <paramref name="x"/>.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="x"/> equals <paramref name="y"/> by value,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    Task<bool> EqualsAsync(T? x, T? y, CancellationToken canceler);

    /// <summary>Computes an identifying hash code for <paramref name="obj"/> based upon value.</summary>
    /// <param name="obj">Object to generate a hash code for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The value computed hash code for <paramref name="obj"/>.</returns>
    Task<int> GetHashCodeAsync(T? obj, CancellationToken canceler);
}
