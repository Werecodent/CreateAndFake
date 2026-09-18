using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="Task"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class TaskCopyHint : CopyHint
{
    private static readonly MethodInfo _GenericCloner = typeof(TaskCopyHint).GetMethod(
        nameof(WrapTaskAsync),
        BindingFlags.NonPublic | BindingFlags.Static
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.TaskHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(Task), typeof(Task<>)];

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        if (source is Task task)
        {
            Type? asGeneric = GenericConverter.AsConcreteType(source.GetType(), typeof(Task<>));
            if (
                asGeneric
                    ?.GetGenericArguments()
                    .Single()
                    .Name.Contains("VoidTaskResult", StringComparison.Ordinal) == false
            )
            {
                return new(
                    _GenericCloner
                        .MakeGenericMethod(asGeneric.GetGenericArguments())
                        .Invoke(null, [task, duplicator])
                );
            }
            else
            {
                return new(WrapPlainTaskAsync(task, duplicator));
            }
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    private static Task WrapPlainTaskAsync(Task task, IDuplicatorChainer duplicator)
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

    private static Task<T> WrapTaskAsync<T>(Task<T> task, IDuplicatorChainer duplicator)
    {
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
