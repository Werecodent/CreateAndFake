using CreateAndFake.Design.Properties;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Comparisons;

/// <summary>Compares <see cref="IValueEquatable"/> <see langword="object"/>s/collections by value.</summary>
/// <typeparam name="T">The supported <see cref="IValueEquatable"/> <see cref="Type"/>.</typeparam>
/// <param name="iterationLimit">Max supported size for iterating sequences.</param>
/// <remarks>Not reflection based.</remarks>
public sealed class ValueComparer<T>(int iterationLimit)
    : ITypeSupporter,
        IComparer<T>,
        IComparer<IEnumerable<T>>,
        IEqualityComparer<T>,
        IEqualityComparer<IEnumerable<T>>,
        IDeepCloneable<ValueComparer<T>>
    where T : IValueEquatable
{
    /// <inheritdoc cref="ValueComparer.Use"/>
    public static ValueComparer<T> Use { get; } = new(DesignDefaults.IterationLimit);

    /// <summary>Handles the actual comparisons.</summary>
    private readonly ValueComparer _comparer = new(iterationLimit);

    /// <inheritdoc/>
    public Type? SupportedType { get; } = typeof(T);

    /// <inheritdoc cref="ValueComparer.Equals(object,object)"/>
    public bool Equals(T? x, T? y)
    {
        return _comparer.Equals(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Equals(object,object)"/>
    public bool Equals(IEnumerable<T?>? x, IEnumerable<T?>? y)
    {
        return _comparer.Equals(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Equals(object,object)"/>
    public bool Equals<TKey>(IDictionary<TKey, T?>? x, IDictionary<TKey, T?>? y)
    {
        return _comparer.Equals(x, y);
    }

    /// <inheritdoc cref="ValueComparer.GetHashCode(object)"/>
    public int GetHashCode(T? obj)
    {
        return _comparer.GetHashCode(obj);
    }

    /// <inheritdoc cref="ValueComparer.GetHashCode(object)"/>
    public int GetHashCode(IEnumerable<T?>? obj)
    {
        return _comparer.GetHashCode(obj);
    }

    /// <inheritdoc cref="ValueComparer.GetHashCode(object)"/>
    public int GetHashCode<TKey>(IDictionary<TKey, T?>? obj)
    {
        return _comparer.GetHashCode(obj);
    }

    /// <inheritdoc cref="ValueComparer.Compare(object,object)"/>
    public int Compare(T? x, T? y)
    {
        return _comparer.Compare(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Compare(object,object)"/>
    public int Compare(IEnumerable<T?>? x, IEnumerable<T?>? y)
    {
        return _comparer.Compare(x, y);
    }

    /// <inheritdoc cref="ValueComparer.Compare(object,object)"/>
    public int Compare<TKey>(IDictionary<TKey, T?>? x, IDictionary<TKey, T?>? y)
    {
        return _comparer.Compare(x, y);
    }

    /// <inheritdoc/>
    public ValueComparer<T> DeepClone()
    {
        return new ValueComparer<T>(iterationLimit);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericTypeConverter.ExpandedName(GetType());
    }
}
