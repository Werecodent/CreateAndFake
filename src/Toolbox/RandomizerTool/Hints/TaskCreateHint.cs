using CreateAndFake.Design;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="Task{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class TaskCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.TaskHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(Task), typeof(TaskCompletionSource<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (type.Inherits<Task>() || typeof(TaskCompletionSource<>).IsInheritedBy(type))
        {
            return new(Create(type, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object? Create(Type type, IRandomizerChainer randomizer)
    {
        if (type.IsGenericType)
        {
            Type content = type.GetGenericArguments().Single();

            return typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(content)
                .Invoke(null, [randomizer.Create(content)]);
        }
        else
        {
            return Task.FromResult(randomizer.Create<int>());
        }
    }
}
