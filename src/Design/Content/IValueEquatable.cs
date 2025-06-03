namespace CreateAndFake.Design.Content;

/// <summary>Provides value equality without changing the default reference equality.</summary>
public interface IValueEquatable
{
    /// <summary>Compares <see langword="this"/> to <paramref name="other"/> by value.</summary>
    /// <param name="other">Instance to compare <see langword="this"/> with.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> is equal to
    ///     <paramref name="other"/> by value, <see langword="false"/> otherwise.
    /// </returns>
    bool ValuesEqual(object? other);

    /// <summary>
    ///     Computes an identifying hash code for <see langword="this"/> based upon value.
    /// </summary>
    /// <returns>The value computed hash code for <see langword="this"/>.</returns>
    int GetValueHash();
}
