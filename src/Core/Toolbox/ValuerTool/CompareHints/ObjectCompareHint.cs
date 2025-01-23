using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.ValuerTool.CompareHints;

/// <summary>Handles comparing objects for <see cref="IValuer"/>.</summary>
/// <param name="scope">Flags used to find properties and fields.</param>
public sealed class ObjectCompareHint(BindingFlags scope) : CompareHint
{
    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, ValuerChainer valuer)
    {
        if (expected == null || actual == null)
        {
            return false;
        }

        Type type = expected.GetType();
        return type.GetProperties(scope).Any(p => p.CanRead)
            || type.GetFields(scope).Length != 0;
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(object? expected, object? actual, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private IEnumerable<Difference> LazyCompare(object expected, object actual, ValuerChainer valuer)
    {
        Type type = expected.GetType();

        foreach (PropertyInfo property in type.GetProperties(scope).Where(p => p.CanRead))
        {
            foreach (Difference diff in valuer.Compare(property.GetValue(expected), property.GetValue(actual)))
            {
                yield return new Difference(property, diff);
            }
        }

        foreach (FieldInfo field in expected.GetType().GetFields(scope))
        {
            foreach (Difference diff in valuer.Compare(field.GetValue(expected), field.GetValue(actual)))
            {
                yield return new Difference(field, diff);
            }
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        Type type = item.GetType();
        int hash = ValueComparer.BaseHash + type.GetHashCode();

        foreach (PropertyInfo property in type.GetProperties(scope).Where(p => p.CanRead))
        {
            hash = hash * ValueComparer.HashMultiplier + valuer.GetHashCode(property.GetValue(item));
        }

        foreach (FieldInfo field in type.GetFields(scope))
        {
            hash = hash * ValueComparer.HashMultiplier + valuer.GetHashCode(field.GetValue(item));
        }

        return hash;
    }
}
