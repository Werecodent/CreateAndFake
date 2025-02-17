using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

/// <summary>Handles cloning <see cref="Task"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class TaskCopyHint : CopyHint
{
    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        if (source is Task task && task.GetType().IsGenericType)
        {
            return new(Copy(task, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <inheritdoc cref="CopyHint{T}.Copy"/>
    private static object Copy(Task source, DuplicatorChainer duplicator)
    {
        PropertyInfo resultHolder = source.GetType().GetProperty(nameof(Task<object>.Result))!;

        return typeof(TaskCopyHint)
            .GetMethod(nameof(NewInstanceFromResult), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(resultHolder.PropertyType)
            .Invoke(null, [duplicator.Copy(resultHolder.GetValue(source))])!;
    }

    /// <summary>Forces a <c>Task</c> instance that isn't cached.</summary>
    /// <typeparam name="T"><c>Task</c> result type.</typeparam>
    /// <param name="result"><typeparamref name="T"/> value to return from the <c>Task</c>.</param>
    /// <returns>The created <c>Task</c>.</returns>
    private static Task<T> NewInstanceFromResult<T>(T result)
    {
        TaskCompletionSource<T> wrapper = new();
        _ = wrapper.TrySetResult(result);
        return wrapper.Task;
    }
}
