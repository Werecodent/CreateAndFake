namespace CreateAndFake.Design.Content;

/// <summary>Compares objects/collections of <c>Type</c> <typeparamref name="T"/> by value.</summary>
/// <typeparam name="T"><c>Type</c> of the objects to compare.</typeparam>
public sealed class ValueComparer<T> :
    IComparer<T>,
    IComparer<IEnumerable<T>>,
    IEqualityComparer<T>,
    IEqualityComparer<IEnumerable<T>>
    where T : IValueEquatable
{
    /// <inheritdoc cref="ValueComparer.Use"/>
    public static ValueComparer<T> Use { get; } = new ValueComparer<T>();

    /// <inheritdoc cref="ValueComparer.Equals(object,object)"/>
    public bool Equals(T? x, T? y)
    {
        return ValueComparer.Use.Equals(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Equals(object,object)"/>
    public bool Equals(IEnumerable<T?>? x, IEnumerable<T?>? y)
    {
        return ValueComparer.Use.Equals(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Equals(object,object)"/>
    public bool Equals<TKey>(IDictionary<TKey, T?>? x, IDictionary<TKey, T?>? y)
    {
        return ValueComparer.Use.Equals(x, y);
    }

    /// <inheritdoc cref="ValueComparer.GetHashCode(object)"/>
    public int GetHashCode(T? obj)
    {
        return ValueComparer.Use.GetHashCode(obj);
    }

    /// <inheritdoc cref="ValueComparer.GetHashCode(object)"/>
    public int GetHashCode(IEnumerable<T?>? obj)
    {
        return ValueComparer.Use.GetHashCode(obj);
    }

    /// <inheritdoc cref="ValueComparer.GetHashCode(object)"/>
    public int GetHashCode<TKey>(IDictionary<TKey, T?>? obj)
    {
        return ValueComparer.Use.GetHashCode(obj);
    }

    /// <inheritdoc cref="ValueComparer.Compare(object,object)"/>
    public int Compare(T? x, T? y)
    {
        return ValueComparer.Use.Compare(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Compare(object,object)"/>
    public int Compare(IEnumerable<T?>? x, IEnumerable<T?>? y)
    {
        return ValueComparer.Use.Compare(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Compare(object,object)"/>
    public int Compare<TKey>(IDictionary<TKey, T?>? x, IDictionary<TKey, T?>? y)
    {
        return ValueComparer.Use.Compare(x, y);
    }
}
