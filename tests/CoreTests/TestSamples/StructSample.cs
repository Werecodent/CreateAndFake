using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Content;

#pragma warning disable IDE0250 // Struct can be made 'readonly': For testing.
#pragma warning disable IDE0251 // Member can be made 'readonly': For testing.

namespace CreateAndFakeTests.TestSamples;

[ExcludeFromCodeCoverage]
public struct StructSample(string stringValue) : IEquatable<StructSample>, IComparable<StructSample>, IComparable
{
    public string StringValue { get; } = stringValue;

    public int CompareTo(StructSample other)
    {
        return ValueComparer.Use.Compare(this, other);
    }

    public int CompareTo(object obj)
    {
        return ValueComparer.Use.Compare(this, obj);
    }

    public override bool Equals(object obj)
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

#pragma warning restore IDE0250 // Struct can be made 'readonly'
#pragma warning restore IDE0251 // Member can be made 'readonly'
