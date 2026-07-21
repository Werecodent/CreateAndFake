using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public struct StructSample(string stringValue)
    : IEquatable<StructSample>,
        IComparable<StructSample>,
        IComparable
{
    public string StringValue { get; } = stringValue;

    public int CompareTo(StructSample other)
    {
        return ValueComparer.Use.Compare(this, other);
    }

    public int CompareTo(object? obj)
    {
        return ValueComparer.Use.Compare(this, obj);
    }

    public override bool Equals(object? obj)
    {
        return (obj is StructSample sample) && Equals(sample);
    }

    public bool Equals(StructSample other)
    {
        return ValueComparer.Use.Equals(StringValue, other.StringValue);
    }

    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(StringValue);
    }

    public static bool operator ==(StructSample left, StructSample right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StructSample left, StructSample right)
    {
        return !(left == right);
    }

    public static bool operator <(StructSample left, StructSample right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(StructSample left, StructSample right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(StructSample left, StructSample right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(StructSample left, StructSample right)
    {
        return left.CompareTo(right) >= 0;
    }
}
