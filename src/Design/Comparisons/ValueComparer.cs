using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Properties;

namespace CreateAndFake.Design.Comparisons;

/// <summary>
///     Compares <see langword="object"/>s/collections
///     by value via <see cref="IValueEquatable"/> if possible.
/// </summary>
/// <param name="iterationLimit">Max supported size for iterating sequences.</param>
/// <remarks>Not reflection based.</remarks>
public sealed class ValueComparer(int iterationLimit)
    : IComparer,
        IComparer<object?>,
        IComparer<IValueEquatable?>,
        IComparer<IEnumerable?>,
        IComparer<IDictionary?>,
        IEqualityComparer,
        IEqualityComparer<object?>,
        IEqualityComparer<IValueEquatable?>,
        IEqualityComparer<IEnumerable?>,
        IEqualityComparer<IDictionary?>,
        IDeepCloneable
{
    /// <summary>Hash code used for <see langword="null"/> values.</summary>
    public static int NullHash { get; } = 0;

    /// <summary>Starting hash code value.</summary>
    public static int BaseHash { get; } = 1009;

    /// <summary>Multiplier for computing hash codes.</summary>
    public static int HashMultiplier { get; } = 92821;

    /// <summary>Default instance to use for comparing by value.</summary>
    public static ValueComparer Use { get; } = new(DesignDefaults.IterationLimit);

    /// <summary>Determines if <paramref name="x"/> equals <paramref name="y"/> by value.</summary>
    /// <param name="x">The <see langword="object"/> to compare with <paramref name="y"/>.</param>
    /// <param name="y">The <see langword="object"/> to compare with <paramref name="x"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="x"/> equals
    ///     <paramref name="y"/> by value, <see langword="false"/> otherwise.
    /// </returns>
    public new bool Equals(object? x, object? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }
        else if (x is null || y is null)
        {
            return false;
        }
        else if (x is IValueEquatable asValue)
        {
            return Equals(asValue, y as IValueEquatable);
        }
        else if (x is IEnumerable asEnum)
        {
            return Equals(asEnum, y as IEnumerable);
        }
        else
        {
            return x.Equals(y);
        }
    }

    /// <inheritdoc cref="Equals(object,object)"/>
    public bool Equals(IValueEquatable? x, IValueEquatable? y)
    {
        return x?.ValuesEqual(y) ?? y?.ValuesEqual(x) ?? true;
    }

    /// <inheritdoc cref="Equals(object,object)"/>
    public bool Equals(IEnumerable? x, IEnumerable? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }
        else if (x is null || y is null)
        {
            return false;
        }
        else if (x is IDictionary asDict)
        {
            return Equals(asDict, y as IDictionary);
        }
        else
        {
            return EqualsBySequence(x, y);
        }
    }

    /// <inheritdoc cref="Equals(object,object)"/>
    public bool Equals(IDictionary? x, IDictionary? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }
        else if (x is null || y is null)
        {
            return false;
        }
        else if (x.Count != y.Count)
        {
            return false;
        }
        else
        {
            foreach (DictionaryEntry entry in x)
            {
                if (!y.Contains(entry.Key) || !Equals(entry.Value, y[entry.Key]))
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <inheritdoc cref="Equals(object,object)"/>
    private bool EqualsBySequence(IEnumerable x, IEnumerable y)
    {
        if (x is string)
        {
            return x.Equals(y);
        }

        IEnumerator? xGen = null;
        IEnumerator? yGen = null;
        try
        {
            xGen = x.GetEnumerator();
            yGen = y.GetEnumerator();

            int i = 0;
            while (xGen.MoveNext())
            {
                ArgumentGuard.ThrowUponIterationLimit(i++, iterationLimit);
                if (!yGen.MoveNext() || !Equals(xGen.Current, yGen.Current))
                {
                    return false;
                }
            }
            return !yGen.MoveNext();
        }
        finally
        {
            Disposer.Cleanup(xGen, yGen);
        }
    }

    /// <summary>
    ///     Computes an identifying hash code for <paramref name="items"/> based upon value.
    /// </summary>
    /// <param name="items">
    ///     Bundled <see langword="object"/>s to generate a single hash code for.
    /// </param>
    /// <returns>The value computed hash code for <paramref name="items"/>.</returns>
    public int GetHashCode(params IEnumerable<object?>? items)
    {
        return GetHashCode((IEnumerable?)items);
    }

    /// <summary>
    ///     Computes an identifying hash code for <paramref name="obj"/> based upon value.
    /// </summary>
    /// <param name="obj">The <see langword="object"/> to generate a hash code for.</param>
    /// <returns>The value computed hash code for <paramref name="obj"/>.</returns>
    public int GetHashCode(object? obj)
    {
        if (obj is null)
        {
            return NullHash;
        }
        else if (obj is IValueEquatable asValue)
        {
            return GetHashCode(asValue);
        }
        else if (obj is IEnumerable asEnum)
        {
            return GetHashCode(asEnum);
        }
        else
        {
            return obj.GetHashCode();
        }
    }

    /// <inheritdoc cref="GetHashCode(object)"/>
    public int GetHashCode(IValueEquatable? obj)
    {
        return obj?.GetValueHash() ?? NullHash;
    }

    /// <inheritdoc cref="GetHashCode(object)"/>
    public int GetHashCode(IEnumerable? obj)
    {
        if (obj is null)
        {
            return NullHash;
        }
        else if (obj is string)
        {
            return obj.GetHashCode();
        }
        else if (obj is IDictionary asDict)
        {
            return GetHashCode(asDict);
        }
        else
        {
            int i = 0;
            int hash = BaseHash;
            foreach (object item in obj)
            {
                ArgumentGuard.ThrowUponIterationLimit(i++, iterationLimit);
                hash = hash * HashMultiplier + GetHashCode(item);
            }
            return hash;
        }
    }

    /// <inheritdoc cref="GetHashCode(object)"/>
    public int GetHashCode(IDictionary? obj)
    {
        if (obj is null)
        {
            return NullHash;
        }

        int hash = BaseHash;
        foreach (DictionaryEntry item in obj)
        {
            hash += GetHashCode(item.Key);
            hash += GetHashCode(item.Value);
        }
        return hash;
    }

    /// <summary>
    ///     Compares <paramref name="x"/> and <paramref name="y"/> by their value hash for sorting.
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

    /// <inheritdoc cref="Compare(object,object)"/>
    public int Compare(IValueEquatable? x, IValueEquatable? y)
    {
        return ReferenceEquals(x, y) ? 0 : GetHashCode(x).CompareTo(GetHashCode(y));
    }

    /// <inheritdoc cref="Compare(object,object)"/>
    public int Compare(IEnumerable? x, IEnumerable? y)
    {
        return ReferenceEquals(x, y) ? 0 : GetHashCode(x).CompareTo(GetHashCode(y));
    }

    /// <inheritdoc cref="Compare(object,object)"/>
    public int Compare(IDictionary? x, IDictionary? y)
    {
        return ReferenceEquals(x, y) ? 0 : GetHashCode(x).CompareTo(GetHashCode(y));
    }

    /// <inheritdoc/>
    public IDeepCloneable DeepClone()
    {
        return new ValueComparer(iterationLimit);
    }
}
