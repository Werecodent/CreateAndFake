using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="Task{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class TaskCreateHint : CreateHint
{
    /// <summary>Method used to create completed tasks.</summary>
    /// <remarks>Dynamic can't be used due to creating the wrong type with subclasses.</remarks>
    private static readonly MethodInfo _TaskMaker = typeof(Task).GetMethod(
        nameof(Task.FromResult)
    )!;

    /// <summary>Method used to create completed sources.</summary>
    /// <remarks>Dynamic can't be used due to creating the wrong type with subclasses.</remarks>
    private static readonly MethodInfo _CompletionMaker = typeof(TaskCreateHint).GetMethod(
        nameof(MakeCompletionSource),
        BindingFlags.NonPublic | BindingFlags.Static
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.TaskHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(Task), typeof(Task<>), typeof(TaskCompletionSource<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = GenericConverter.AsGenericBase(type);

        if (asGeneric == typeof(TaskCompletionSource<>))
        {
            Type contentType = type.GetGenericArguments().Single();
            return new(
                _CompletionMaker
                    .MakeGenericMethod(contentType)
                    .Invoke(null, [randomizer.Create(contentType)])
            );
        }
        else if (asGeneric == typeof(Task<>))
        {
            Type contentType = type.GetGenericArguments().Single();
            return new(
                _TaskMaker
                    .MakeGenericMethod(contentType)
                    .Invoke(null, [randomizer.Create(contentType)])
            );
        }
        else if (type == typeof(Task))
        {
            return new(Task.FromResult(randomizer.Create<int>()));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    private static TaskCompletionSource<T> MakeCompletionSource<T>(T content)
    {
        TaskCompletionSource<T> result = new();
        result.SetResult(content);
        return result;
    }
}
