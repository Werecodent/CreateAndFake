namespace CreateAndFake.Design.Content;

/// <summary>Provides value equality without changing the default reference equality.</summary>
public interface IValueEquatable
{
    /// <summary>Compares <c>this</c> to <paramref name="other"/> by value.</summary>
    /// <param name="other">Instance to compare <c>this</c> with.</param>
    /// <returns>
    ///     <c>true</c> if <c>this</c> is equal to <paramref name="other"/> by value; 
    ///     <c>false</c> otherwise.
    /// </returns>
    bool ValuesEqual(object? other);

    /// <summary>Computes an identifying hash code for <c>this</c> based upon value.</summary>
    /// <returns>The value computed hash code for <c>this</c>.</returns>
    int GetValueHash();
}
