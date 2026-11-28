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
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(Task), typeof(Task<>), typeof(TaskCompletionSource<>)];

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
        else if (TypeDescriber.For(source).Inherits(typeof(TaskCompletionSource<>)))
        {
            return new(ConvertSource((dynamic)source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <inheritdoc cref="TryCopy"/>
    private static Task WrapPlainTaskAsync(Task source, IDuplicatorChainer duplicator)
    {
        if (source.IsCanceled)
        {
            return Task.FromCanceled(new CancellationToken(true));
        }
        else if (source.IsFaulted)
        {
            return Task.FromException(duplicator.Copy(source.Exception.InnerException!));
        }
        else if (source.IsCompleted)
        {
            return Task.CompletedTask;
        }
        else
        {
            return Task.Run(() => source);
        }
    }

#pragma warning disable CA1849, MA0042, VSTHRD002, VSTHRD103 // Completion verified.

    /// <inheritdoc cref="TryCopy"/>
    private static Task<T> WrapTaskAsync<T>(Task<T> source, IDuplicatorChainer duplicator)
    {
        if (source.IsCanceled)
        {
            return Task.FromCanceled<T>(new CancellationToken(true));
        }
        else if (source.IsFaulted)
        {
            return Task.FromException<T>(duplicator.Copy(source.Exception.InnerException!));
        }
        else if (source.IsCompleted)
        {
            return Task.FromResult(duplicator.Copy(source.Result));
        }
        else
        {
            return Task.Run(async () => duplicator.Copy(await source.ConfigureAwait(false)));
        }
    }

    /// <inheritdoc cref="TryCopy"/>
    private static TaskCompletionSource<T> ConvertSource<T>(
        TaskCompletionSource<T> source,
        IDuplicatorChainer duplicator
    )
    {
        TaskCompletionSource<T> result = new();
        if (source.Task.IsCanceled)
        {
            result.SetCanceled();
        }
        else if (source.Task.IsFaulted)
        {
            result.SetException(duplicator.Copy(source.Task.Exception.InnerException!));
        }
        else
        {
            result.SetResult(duplicator.Copy(source.Task.Result));
        }
        return result;
    }

#pragma warning restore CA1849, MA0042, VSTHRD103
}
