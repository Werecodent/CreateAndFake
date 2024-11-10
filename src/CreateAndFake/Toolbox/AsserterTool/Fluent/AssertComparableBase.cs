namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common comparables assertion calls.</summary>
/// <param name="value"><inheritdoc cref="Value" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertComparableBase<T>(AsserterOptions options, IComparable? value)
    : AssertObjectBase<T>(options, value) where T : AssertComparableBase<T>
{
    /// <summary>Value to run assertion checks with.</summary>
    protected IComparable? Value { get; } = value;

    /// <summary>Verifies <c>value</c> &gt; <paramref name="expected"/>.</summary>
    /// <inheritdoc cref="HandleMathCheck"/>
    public virtual AssertChainer<T> GreaterThan(IComparable expected, string? details = null)
    {
        return HandleMathCheck(() => Value!.CompareTo(expected) > 0, "greater than", expected, details);
    }

    /// <summary>Verifies <c>value</c> &gt;= <paramref name="expected"/>.</summary>
    /// <inheritdoc cref="HandleMathCheck"/>
    public virtual AssertChainer<T> GreaterThanOrEqualTo(IComparable expected, string? details = null)
    {
        return HandleMathCheck(() => Value!.CompareTo(expected) >= 0, "greater than or equal to", expected, details);
    }

    /// <summary>Verifies <c>value</c> &gt; <paramref name="expected"/> or equals by value.</summary>
    /// <inheritdoc cref="HandleMathCheck"/>
    public virtual AssertChainer<T> GreaterThanOrIs(IComparable expected, string? details = null)
    {
        return Options.Valuer.Equals(Value, expected)
            ? ToChainer()
            : GreaterThanOrEqualTo(expected, details);
    }

    /// <summary>Verifies <c>value</c> &lt; <paramref name="expected"/>.</summary>
    /// <inheritdoc cref="HandleMathCheck"/>
    public virtual AssertChainer<T> LessThan(IComparable expected, string? details = null)
    {
        return HandleMathCheck(() => Value!.CompareTo(expected) < 0, "less than", expected, details);
    }

    /// <summary>Verifies value is &lt;= <paramref name="expected"/>.</summary>
    /// <inheritdoc cref="HandleMathCheck"/>
    public virtual AssertChainer<T> LessThanOrEqualTo(IComparable expected, string? details = null)
    {
        return HandleMathCheck(() => Value!.CompareTo(expected) <= 0, "less than or equal to", expected, details);
    }

    /// <summary>Verifies <c>value</c> &lt; <paramref name="expected"/> or equals by value.</summary>
    /// <inheritdoc cref="HandleMathCheck"/>
    public virtual AssertChainer<T> LessThanOrIs(IComparable expected, string? details = null)
    {
        return Options.Valuer.Equals(Value, expected)
            ? ToChainer()
            : LessThanOrEqualTo(expected, details);
    }

    /// <summary>Verifies <paramref name="min"/> &lt;= <c>value</c> &lt;= <paramref name="max"/>.</summary>
    /// <param name="min">Inclusive lower bound.</param>
    /// <param name="max">Inclusive upper bound.</param>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    public virtual AssertChainer<T> InRange(IComparable min, IComparable max, string? details = null)
    {
        if (Value == null)
        {
            throw new AssertException(
                $"Value was null and not in range [{min}, {max}].",
                details, Options.Gen.InitialSeed);
        }
        else if (min == null || max == null)
        {
            throw new AssertException(
                $"Min {min} or max {max} was null and not valid for math comparison check.",
                details, Options.Gen.InitialSeed, Value.ToString());
        }
        else if (Value.CompareTo(min) < 0 || Value.CompareTo(max) > 0)
        {
            throw new AssertException(
                $"Value was not in range [{min}, {max}].",
                details, Options.Gen.InitialSeed, Value.ToString());
        }
        else
        {
            return ToChainer();
        }
    }

    /// <summary>Verifies <c>value</c> matches the <paramref name="math"/>.</summary>
    /// <param name="math">Math used to check the <c>value</c>.</param>
    /// <param name="description">Math description to use for error message.</param>
    /// <param name="expected">Value to compare with.</param>
    /// <param name="details">Optional failure details to include.</param>
    /// <returns>Chainer to make additional assertions with.</returns>
    /// <exception cref="AssertException">If <c>value</c> does not match <paramref name="math"/>.</exception>
    private AssertChainer<T> HandleMathCheck(Func<bool> math, string description, IComparable expected, string? details)
    {
        if (Value == null)
        {
            throw new AssertException(
                $"Value was null and not {description} '{expected}'.",
                details, Options.Gen.InitialSeed);
        }
        else if (expected == null)
        {
            throw new AssertException(
                $"Expected was null and not valid for math comparison check.",
                details, Options.Gen.InitialSeed, Value.ToString());
        }
        else if (!math())
        {
            throw new AssertException(
                $"Value was not {description} '{expected}'.",
                details, Options.Gen.InitialSeed, Value.ToString());
        }
        else
        {
            return ToChainer();
        }
    }
}
