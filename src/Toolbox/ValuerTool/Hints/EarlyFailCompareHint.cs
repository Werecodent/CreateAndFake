using System.Collections;
using System.Collections.Frozen;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles basic <see cref="Type"/> compare issues for <see cref="IValuer"/>.</summary>
public sealed class EarlyFailCompareHint : CompareHint
{
    /// <summary>Specific types to control via this hint.</summary>
    private static readonly FrozenSet<Type> _SupportedTypes = FrozenSet.ToFrozenSet(
        [typeof(string), typeof(object)]
    );

    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, ValuerChainer valuer)
    {
        return expected == null
            || actual == null
            || Supports(expected.GetType(), actual.GetType())
            || expected is Delegate
            || expected is Type;
    }

    /// <inheritdoc cref="CompareHint.Supports"/>
    private static bool Supports(Type expected, Type actual)
    {
        return (
                expected != actual
                && !(expected.Inherits<IEnumerable>() && actual.Inherits<IEnumerable>())
            )
            || expected.IsPrimitive
            || expected.IsEnum
            || _SupportedTypes.Contains(expected);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerChainer valuer
    )
    {
        if (expected == null && actual == null)
        {
            yield break;
        }
        else if (expected == null || actual == null)
        {
            yield return new Difference(expected, actual);
        }
        else if (expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }
        else if (!expected.Equals(actual))
        {
            yield return new Difference(expected, actual);
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, ValuerChainer valuer)
    {
        return ValueComparer.Use.GetHashCode(item);
    }
}
