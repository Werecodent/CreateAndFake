using System.Runtime.CompilerServices;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <inheritdoc/>
public sealed class TaskCompareHint : CompareHint<Task>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.TaskHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        Task expected,
        Task actual,
        IValuerChainer chainer
    )
    {
        return chainer.Compare(ExtractData(expected), ExtractData(actual));
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        Task expected,
        Task actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return HandleCompareAsync(expected, actual, chainer, canceler);
    }

    /// <inheritdoc cref="CompareAsync"/>
    private static async IAsyncEnumerable<Difference> HandleCompareAsync(
        Task expected,
        Task actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        await foreach (
            Difference diff in chainer
                .CompareAsync(
                    await ExtractDataAsync(expected, chainer, canceler).ConfigureAwait(false),
                    await ExtractDataAsync(actual, chainer, canceler).ConfigureAwait(false),
                    canceler
                )
                .ConfigureAwait(false)
        )
        {
            yield return diff;
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(Task item, IValuerChainer chainer)
    {
        return chainer.GetHashCode(ExtractData(item));
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        Task item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return await chainer
            .GetHashCodeAsync(
                await ExtractDataAsync(item, chainer, canceler).ConfigureAwait(false),
                canceler
            )
            .ConfigureAwait(false);
    }

    /// <summary>Converts the <paramref name="item"/> to comparable data.</summary>
    /// <param name="item">Instance intended to be compared.</param>
    /// <returns>The found data to use for comparisons.</returns>
    private static object? ExtractData(Task item)
    {
        if (item.Status == TaskStatus.RanToCompletion && IsGenericTask(item))
        {
            return ((dynamic)item).Result;
        }
        else
        {
            return (item.Status, item.Exception);
        }
    }

    /// <inheritdoc cref="ExtractData"/>
    /// <inheritdoc cref="GetHashCodeAsync"/>
    private static async Task<object?> ExtractDataAsync(
        Task item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        if (
            !item.IsCompleted
            && await Task.WhenAny(item, Task.Delay(chainer.Options.AsyncTimeout, canceler))
                .ConfigureAwait(false) != item
        )
        {
            canceler.ThrowIfCancellationRequested();
            throw new EngineException(
                $"Attempting to await the {TypeHelper.ExpandedName(item)} exceeded the timeout "
                    + $"({nameof(ValuerOptions.AsyncTimeout)}) of '{chainer.Options.AsyncTimeout}'."
            );
        }

        if (IsGenericTask(item))
        {
            return await ((dynamic)item).ConfigureAwait(false);
        }
        else
        {
            await item.ConfigureAwait(false);
            return (item.Status, item.Exception);
        }
    }

    /// <summary>Determines if the <paramref name="item"/> is a <see cref="Task{T}"/>.</summary>
    /// <param name="item">Instance to check.</param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="item"/> is generic, false otherwise.
    /// </returns>
    private static bool IsGenericTask(Task item)
    {
        return TypeHelper
                .AsConcreteType(item.GetType(), typeof(Task<>))
                ?.GetGenericArguments()
                .Single()
                .Name.Contains("VoidTaskResult", StringComparison.Ordinal) == false;
    }
}
