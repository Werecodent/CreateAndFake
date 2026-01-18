using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing stateless objects for <see cref="IValuer"/>.</summary>
public sealed class StatelessCompareHint : CompareHint
{
    /// <summary>Flags used to find properties and fields.</summary>
    private const BindingFlags _Scope =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        if (expected == null || actual == null)
        {
            return false;
        }

        Type type = expected.GetType();
        return !TypeDescriber.GetAllProperties(type, _Scope).Any(p => p.CanRead)
            && !TypeDescriber.GetAllFields(type, _Scope).Any();
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        return [];
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item);

        return item.GetType().GetHashCode();
    }
}
