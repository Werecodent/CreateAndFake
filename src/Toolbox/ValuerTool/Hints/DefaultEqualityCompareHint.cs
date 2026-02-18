using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles basic <see cref="Type"/> compare issues for <see cref="IValuer"/>.</summary>
public sealed class DefaultEqualityCompareHint : CompareHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.DefaultEqualityHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        Type expectedType = expected.GetType();
        Type actualType = actual.GetType();
        return (
                expectedType != actualType
                && !(expectedType.Inherits<IEnumerable>() && actualType.Inherits<IEnumerable>())
            )
            || expectedType.IsPrimitive
            || expectedType.IsEnum
            || chainer.Options.DefaultEqualityTypes.Contains(expectedType)
            || expected is Delegate
            || expectedType == typeof(object);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
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
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        return ValueComparer.Use.GetHashCode(item);
    }
}
