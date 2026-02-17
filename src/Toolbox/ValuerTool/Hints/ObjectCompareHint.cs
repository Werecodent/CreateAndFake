using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing objects for <see cref="IValuer"/>.</summary>
/// <param name="onlyPublic">If private members are excluded.</param>
public abstract class ObjectCompareHint(bool onlyPublic) : CompareHint
{
    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer valuer)
    {
        Type type = expected.GetType();
        return TypeDescriber.GetAllProperties(type, onlyPublic).Any(p => p.CanRead)
            || TypeDescriber.GetAllFields(type, onlyPublic).Any();
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer valuer
    )
    {
        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private IEnumerable<Difference> LazyCompare(
        object expected,
        object actual,
        IValuerChainer valuer
    )
    {
        Type type = expected.GetType();

        foreach (
            PropertyInfo property in TypeDescriber
                .GetAllProperties(type, onlyPublic)
                .Where(p => p.CanRead)
        )
        {
            foreach (
                Difference diff in valuer.Compare(
                    property.GetValue(expected),
                    property.GetValue(actual)
                )
            )
            {
                yield return new Difference(property, diff);
            }
        }

        foreach (FieldInfo field in TypeDescriber.GetAllFields(type, onlyPublic))
        {
            foreach (
                Difference diff in valuer.Compare(field.GetValue(expected), field.GetValue(actual))
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
        IValuerChainer valuer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        Type type = expected.GetType();

        foreach (
            PropertyInfo property in TypeDescriber
                .GetAllProperties(type, onlyPublic)
                .Where(p => p.CanRead)
        )
        {
            await foreach (
                Difference diff in valuer
                    .CompareAsync(property.GetValue(expected), property.GetValue(actual))
                    .WithCancellation(canceler)
                    .ConfigureAwait(false)
            )
            {
                yield return new Difference(property, diff);
            }
        }

        foreach (FieldInfo field in TypeDescriber.GetAllFields(type, onlyPublic))
        {
            await foreach (
                Difference diff in valuer
                    .CompareAsync(field.GetValue(expected), field.GetValue(actual))
                    .WithCancellation(canceler)
                    .ConfigureAwait(false)
            )
            {
                yield return new Difference(field, diff);
            }
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer valuer)
    {
        Type type = item.GetType();
        int hash = ValueComparer.BaseHash + type.GetHashCode();

        foreach (
            PropertyInfo property in TypeDescriber
                .GetAllProperties(type, onlyPublic)
                .Where(p => p.CanRead)
        )
        {
            hash =
                hash * ValueComparer.HashMultiplier + valuer.GetHashCode(property.GetValue(item));
        }

        foreach (FieldInfo field in TypeDescriber.GetAllFields(type, onlyPublic))
        {
            hash = hash * ValueComparer.HashMultiplier + valuer.GetHashCode(field.GetValue(item));
        }

        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        object item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        Type type = item.GetType();
        int hash = ValueComparer.BaseHash + type.GetHashCode();

        foreach (
            PropertyInfo property in TypeDescriber
                .GetAllProperties(type, onlyPublic)
                .Where(p => p.CanRead)
        )
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await valuer
                    .GetHashCodeAsync(property.GetValue(item), canceler)
                    .ConfigureAwait(false);
        }

        foreach (FieldInfo field in TypeDescriber.GetAllFields(type, onlyPublic))
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await valuer
                    .GetHashCodeAsync(field.GetValue(item), canceler)
                    .ConfigureAwait(false);
        }

        return hash;
    }
}
