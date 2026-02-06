using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="Task"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class TaskCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.TaskHint;

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        if (source == Task.CompletedTask)
        {
            return new(Task.CompletedTask);
        }
        else if (source is Task task)
        {
            if (task.GetType().IsGenericType)
            {
                return new(
                    typeof(TaskCopyHint)
                        .GetMethod(nameof(WrapTask), BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(task.GetType().GetGenericArguments())
                        .Invoke(null, [task, duplicator])
                );
            }
            else
            {
                return new(WrapPlainTask(task, duplicator));
            }
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    private static Task WrapPlainTask(Task task, IDuplicatorChainer duplicator)
    {
        if (task.IsCanceled)
        {
            return Task.FromCanceled(new CancellationToken(true));
        }
        else if (task.IsFaulted)
        {
            return Task.FromException(duplicator.Copy(task.Exception));
        }
        else if (task.IsCompleted)
        {
            return Task.CompletedTask;
        }
        else
        {
            return Task.Run(() => task);
        }
    }

#pragma warning disable CA1849, MA0042, VSTHRD103 // Completion verified.

    private static Task<T> WrapTask<T>(Task rawTask, IDuplicatorChainer duplicator)
    {
        Task<T> task = (Task<T>)rawTask;
        if (task.IsCanceled)
        {
            return Task.FromCanceled<T>(new CancellationToken(true));
        }
        else if (task.IsFaulted)
        {
            return Task.FromException<T>(duplicator.Copy(task.Exception));
        }
        else if (task.IsCompleted)
        {
            return Task.FromResult(duplicator.Copy(task.Result));
        }
        else
        {
            return Task.Run(async () => duplicator.Copy(await task.ConfigureAwait(false)));
        }
    }

#pragma warning restore CA1849, MA0042, VSTHRD103
}
