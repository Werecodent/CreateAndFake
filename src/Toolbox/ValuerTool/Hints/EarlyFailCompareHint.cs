using System.Collections;
using System.Collections.Frozen;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles basic <see cref="Type"/> compare issues for <see cref="IValuer"/>.</summary>
public sealed class EarlyFailCompareHint : CompareHint
{
    /// <summary>Specific types to control via this hint.</summary>
    private static readonly FrozenSet<Type> _SupportedTypes = FrozenSet.ToFrozenSet([
        typeof(object),
    ]);

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.EarlyFailHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer valuer)
    {
        return Supports(expected.GetType(), actual.GetType()) || expected is Delegate;
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
        object expected,
        object actual,
        IValuerChainer valuer
    )
    {
        if (expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }
        else if (!expected.Equals(actual))
        {
            yield return new Difference(expected, actual);
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer valuer)
    {
        return ValueComparer.Use.GetHashCode(item);
    }
}
