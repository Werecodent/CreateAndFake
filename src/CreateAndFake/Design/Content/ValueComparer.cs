using System.Collections;

namespace CreateAndFake.Design.Content;

/// <summary>Compares objects/collections by value via <see cref="IValueEquatable"/> if possible.</summary>
public sealed class ValueComparer :
    IComparer,
    IComparer<object>,
    IComparer<IValueEquatable>,
    IComparer<IEnumerable>,
    IComparer<IDictionary>,
    IEqualityComparer,
    IEqualityComparer<object>,
    IEqualityComparer<IValueEquatable>,
    IEqualityComparer<IEnumerable>,
    IEqualityComparer<IDictionary>
{
    /// <summary>Hash used for <c>null</c> values.</summary>
    public static int NullHash { get; } = 0;

    /// <summary>Starting hash value.</summary>
    public static int BaseHash { get; } = 1009;

    /// <summary>Multiplier for computing hashes.</summary>
    public static int HashMultiplier { get; } = 92821;

    /// <summary>Default instance to use for comparing objects by value.</summary>
    public static ValueComparer Use { get; } = new ValueComparer();

    /// <summary>Determines if <paramref name="x"/> equals <paramref name="y"/> by value.</summary>
    /// <param name="x">Object to compare with <paramref name="y"/>.</param>
    /// <param name="y">Object to compare with <paramref name="x"/>.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="x"/> equals <paramref name="y"/> by value; <c>false</c> otherwise.
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
    private bool EqualsBySequence(IEnumerable x, IEnumerable y)
    {
        if (x is string)
        {
            return x.Equals(y);
        }
        else
        {
            IEnumerator xGen = x.GetEnumerator();
            IEnumerator yGen = y.GetEnumerator();

            while (xGen.MoveNext())
            {
                if (!yGen.MoveNext() || !Equals(xGen.Current, yGen.Current))
                {
                    return false;
                }
            }
            return !yGen.MoveNext();
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

    /// <summary>Computes an identifying hash code for <paramref name="items"/> based upon value.</summary>
    /// <param name="items">Bundled objects to generate a single value hash code for.</param>
    /// <returns>The value computed hash code for <paramref name="items"/>.</returns>
    public int GetHashCode(params IEnumerable<object?>? items)
    {
        return GetHashCode((IEnumerable?)items);
    }

    /// <summary>Computes an identifying hash code for <paramref name="obj"/> based upon value.</summary>
    /// <param name="obj">Object to generate a hash code for.</param>
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
            int hash = BaseHash;
            foreach (object item in obj)
            {
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
        else
        {
            int hash = BaseHash;
            foreach (DictionaryEntry item in obj)
            {
                hash += GetHashCode(item.Key);
                hash += GetHashCode(item.Value);
            }
            return hash;
        }
    }

    /// <summary>
    ///     Compares <paramref name="x"/> and <paramref name="y"/> by their value hash for sorting.
    /// </summary>
    /// <param name="x">Object to compare with <paramref name="y"/>.</param>
    /// <param name="y">Object to compare with <paramref name="x"/>.</param>
    /// <returns>
    ///     <para>Positive value if <paramref name="x"/> &gt; <paramref name="y"/>.</para>
    ///     <para>Zero if <paramref name="x"/> = <paramref name="y"/>.</para>
    ///     <para>Negative value if <paramref name="x"/> &lt; <paramref name="y"/>.</para>
    /// </returns>
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
}
