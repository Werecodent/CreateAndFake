using CreateAndFake.Design.Comparisons;

namespace CreateAndFake.FakerTool;

/// <summary>Tracks number of calls.</summary>
public sealed class Times : IEquatable<Times>, IDeepCloneable<Times>
{
    /// <summary>Expected bounds.</summary>
    private readonly int _min,
        _max;

    /// <inheritdoc cref="Times"/>
    /// <param name="count">Upper and lower bound.</param>
    /// <remarks>Sets the expected bounds to a single value.</remarks>
    private Times(int count)
        : this(count, count) { }

    /// <inheritdoc cref="Times"/>
    /// <param name="min">Lower bound.</param>
    /// <param name="max">Upper bound.</param>
    private Times(int min, int max)
    {
        _min = min;
        _max = max;
    }

    /// <inheritdoc/>
    public Times DeepClone()
    {
        return new Times(_min, _max);
    }

    /// <summary>Checks if <paramref name="count"/> is in the expected range.</summary>
    /// <param name="count">Value to verify.</param>
    /// <returns><see langword="true"/> if <paramref name="count"/> is in range, <see langword="false"/> otherwise.</returns>
    internal bool IsInRange(int count)
    {
        return _min <= count && count <= _max;
    }

    /// <summary>Compares <see langword="this"/> to <paramref name="obj"/> by value.</summary>
    /// <param name="obj">Instance to compare <see langword="this"/> with.</param>
    /// <returns>
    ///     <see langword="true"/> if <see langword="this"/> is equal to <paramref name="obj"/> by value, <see langword="false"/> otherwise.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Times);
    }

    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    public bool Equals(Times? other)
    {
        return other != null && _min == other._min && _max == other._max;
    }

    /// <summary>Computes an identifying hash code based upon value.</summary>
    /// <returns>The computed hash code.</returns>
    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(_min, _max);
    }

    /// <summary>Converts <see langword="this"/> to a <see langword="string"/>.</summary>
    /// <returns><see langword="string"/> representation of <see langword="this"/>.</returns>
    public override string ToString()
    {
        string maxValue = (_max != int.MaxValue) ? $"{_max}" : "*";
        return (_min != _max) ? $"[{_min}-{maxValue}]" : maxValue;
    }

    /// <summary>Sets the expected bounds to a single value.</summary>
    /// <param name="count">Upper and lower bound.</param>
    public static implicit operator Times(int count)
    {
        return ToTimes(count);
    }

    /// <summary>Sets the expected bounds to a single value.</summary>
    /// <param name="count">Upper and lower bound.</param>
    public static Times ToTimes(int count)
    {
        return new Times(count);
    }

    /// <summary>Represents <c>0</c> allowed calls.</summary>
    public static Times Never { get; } = Exactly(0);

    /// <summary>Represents <c>1</c> allowed calls.</summary>
    public static Times Once { get; } = Exactly(1);

    /// <summary>Represents any number of allowed calls.</summary>
    public static Times Any { get; } = Min(0);

    /// <summary>Sets the expected bounds to exactly <paramref name="count"/>.</summary>
    /// <param name="count">Upper and lower bound.</param>
    /// <returns>Representation of the bounds.</returns>
    public static Times Exactly(int count)
    {
        return new Times(count);
    }

    /// <summary>Sets the bounds to <paramref name="min"/> and <paramref name="max"/>.</summary>
    /// <param name="min">Lower bound.</param>
    /// <param name="max">Upper bound.</param>
    /// <returns>Representation of the bounds.</returns>
    public static Times Between(int min, int max)
    {
        return new Times(min, max);
    }

    /// <summary>Sets the expected bounds to anything above <paramref name="count"/>.</summary>
    /// <param name="count">Lower bound.</param>
    /// <returns>Representation of the bounds.</returns>
    public static Times Min(int count)
    {
        return Between(count, int.MaxValue);
    }

    /// <summary>Sets the expected bounds to anything below <paramref name="count"/>.</summary>
    /// <param name="count">Upper bound.</param>
    /// <returns>Representation of the bounds.</returns>
    public static Times Max(int count)
    {
        return Between(0, count);
    }
}
