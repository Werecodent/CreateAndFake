using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

#pragma warning disable CA1062 // Temporary

/// <summary>Handles comparing objects for <see cref="IValuer"/>.</summary>
/// <param name="onlyPublic">If private members are excluded.</param>
public abstract class ObjectCompareHint(bool onlyPublic) : CompareHint
{
    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        Type type = expected.GetType();
        return GetAccessibleProperties(type).Any() || GetAccessibleFields(type).Any();
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        return LazyCompare(expected, actual, chainer);
    }

    /// <inheritdoc cref="Compare"/>
    private IEnumerable<Difference> LazyCompare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        Type type = expected.GetType();

        foreach (PropertyInfo property in GetAccessibleProperties(type))
        {
            foreach (
                Difference diff in chainer.Compare(
                    property.GetValue(expected),
                    property.GetValue(actual)
                )
            )
            {
                yield return new Difference(property, diff);
            }
        }

        foreach (FieldInfo field in GetAccessibleFields(type))
        {
            foreach (
                Difference diff in chainer.Compare(field.GetValue(expected), field.GetValue(actual))
            )
            {
                yield return new Difference(field, diff);
            }
        }
    }

    /// <inheritdoc/>
    protected override async IAsyncEnumerable<Difference> CompareAsync(
        object expected,
        object actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        Type type = expected.GetType();

        foreach (PropertyInfo property in GetAccessibleProperties(type))
        {
            await foreach (
                Difference diff in chainer
                    .CompareAsync(property.GetValue(expected), property.GetValue(actual), canceler)
                    .WithCancellation(canceler)
                    .ConfigureAwait(false)
            )
            {
                yield return new Difference(property, diff);
            }
        }

        foreach (FieldInfo field in GetAccessibleFields(type))
        {
            await foreach (
                Difference diff in chainer
                    .CompareAsync(field.GetValue(expected), field.GetValue(actual), canceler)
                    .WithCancellation(canceler)
                    .ConfigureAwait(false)
            )
            {
                yield return new Difference(field, diff);
            }
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        Type type = item.GetType();
        int hash = ValueComparer.BaseHash + type.GetHashCode();

        foreach (PropertyInfo property in GetAccessibleProperties(type))
        {
            hash =
                hash * ValueComparer.HashMultiplier + chainer.GetHashCode(property.GetValue(item));
        }

        foreach (FieldInfo field in GetAccessibleFields(type))
        {
            hash = hash * ValueComparer.HashMultiplier + chainer.GetHashCode(field.GetValue(item));
        }

        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        object item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        Type type = item.GetType();
        int hash = ValueComparer.BaseHash + type.GetHashCode();

        foreach (PropertyInfo property in GetAccessibleProperties(type))
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await chainer
                    .GetHashCodeAsync(property.GetValue(item), canceler)
                    .ConfigureAwait(false);
        }

        foreach (FieldInfo field in GetAccessibleFields(type))
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await chainer
                    .GetHashCodeAsync(field.GetValue(item), canceler)
                    .ConfigureAwait(false);
        }

        return hash;
    }

    private IEnumerable<PropertyInfo> GetAccessibleProperties(Type? type)
    {
        return (
            onlyPublic
                ? InheritanceTracker.For(type).Properties.OnlyPublic
                : InheritanceTracker.For(type).Properties.All
        ).Where(p => p.CanRead);
    }

    private IEnumerable<FieldInfo> GetAccessibleFields(Type? type)
    {
        return onlyPublic
            ? InheritanceTracker.For(type).Fields.OnlyPublic
            : InheritanceTracker.For(type).Fields.All;
    }
}

#pragma warning restore CA1062
