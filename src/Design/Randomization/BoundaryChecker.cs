namespace Werecodent.CreateAndFake.Design.Randomization;

/// <summary>Enforces boundaries for generating values.</summary>
/// <remarks>
///     Required branching behavior due to <see cref="IntPtr"/> and <see cref="UIntPtr"/>
///     not inheriting <see cref="IComparable"/> in legacy .NET.
/// </remarks>
internal static class BoundaryChecker
{
    /// <summary>Ensures <c>max &gt;= 0</c>.</summary>
    /// <param name="zero">The <c>0</c> value of <paramref name="max"/>'s <see cref="Type"/>.</param>
    /// <param name="max">Value being proposed as the maximum value to generate.</param>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="max"/> &lt; <c>0</c>.</exception>
    internal static void EnforcePositiveMax(object zero, object max)
    {
        bool isInvalid;
        switch (max)
        {
            case IntPtr intPtrMax:
                isInvalid = intPtrMax.ToInt64() < 0L;
                break;
            case UIntPtr:
                isInvalid = false;
                break;
            default:
                isInvalid = ((IComparable)zero).CompareTo(max) > 0;
                break;
        }

        if (isInvalid)
        {
            throw new ArgumentOutOfRangeException(
                nameof(max),
                max,
                $"Must be greater than or equal to: '{zero}'."
            );
        }
    }

    /// <summary>Ensures <c>min &lt;= max</c>.</summary>
    /// <param name="min">Value being proposed as the minimum value to generate.</param>
    /// <param name="max">Value being proposed as the maximum value to generate.</param>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="max"/> &lt; <c>0</c>.</exception>
    internal static void EnforceRange(object min, object max)
    {
        bool isInvalid;
        switch (min)
        {
            case IntPtr intPtrMin:
                isInvalid = intPtrMin.ToInt64() > ((IntPtr)max).ToInt64();
                break;
            case UIntPtr uIntPtrMin:
                isInvalid = uIntPtrMin.ToUInt64() > ((UIntPtr)max).ToUInt64();
                break;
            default:
                isInvalid = ((IComparable)min).CompareTo(max) > 0;
                break;
        }

        if (isInvalid)
        {
            throw new ArgumentOutOfRangeException(
                nameof(max),
                max,
                $"Must be greater than or equal to the specified min: '{min}'."
            );
        }
    }

    /// <summary>Constrains value to prevent issues stemming from scaling imprecision.</summary>
    /// <param name="value">The generated value to ensure is within range.</param>
    /// <param name="zero">The <c>0</c> value of <paramref name="max"/>'s <see cref="Type"/>.</param>
    /// <param name="max">Positive exclusive upper boundary for the value.</param>
    /// <returns><list type="bullet">
    ///     <item><paramref name="value"/> if <c>value &lt; max</c>.</item>
    ///     <item><paramref name="zero"/> if <c>value &gt;= max</c>.</item>
    /// </list></returns>
    internal static object BindToMax(object value, object zero, object max)
    {
        if (value is IComparable result)
        {
            return result.CompareTo(max) < 0 ? result : zero;
        }
        else
        {
            return value;
        }
    }

    /// <summary>Constrains value to prevent issues stemming from scaling imprecision.</summary>
    /// <param name="value">The generated value to ensure is within range.</param>
    /// <param name="min">Inclusive lower boundary for the value.</param>
    /// <param name="max">Inclusive upper boundary for the value.</param>
    /// <returns><list type="bullet">
    ///     <item><paramref name="value"/> if <c>value &lt;= max</c>.</item>
    ///     <item><paramref name="min"/> if <c>value &gt; max</c>.</item>
    /// </list></returns>
    internal static object BindToRange(object value, object min, object max)
    {
        if (value is IComparable result)
        {
            return result.CompareTo(max) <= 0 ? result : min;
        }
        else
        {
            return value;
        }
    }
}
