using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="Task"/> instances for <see cref="IValuer"/>.</summary>
public sealed class TaskCompareHint : CompareHint<Task>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        Task? expected,
        Task? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.Compare(ExtractResult(expected), ExtractResult(actual));
    }

    /// <inheritdoc/>
    protected override int GetHashCode(Task? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.GetHashCode(ExtractResult(item));
    }

    /// <summary>Retrieves the result from a task.</summary>
    /// <param name="item">Given task.</param>
    /// <returns>Result if possible; status otherwise.</returns>
    private static object? ExtractResult(Task? item)
    {
        if (item == null)
        {
            return null;
        }
        else if (item.Status != TaskStatus.RanToCompletion || !item.GetType().IsGenericType)
        {
            return (item.Status, item.Exception);
        }
        else
        {
            return item.GetType().GetProperty(nameof(Task<object>.Result))!.GetValue(item);
        }
    }
}
