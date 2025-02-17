using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing <see cref="Task{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class TaskCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

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
    private static object? Create(Type type, RandomizerChainer randomizer)
    {
        if (type.IsGenericType)
        {
            Type content = type.GetGenericArguments().Single();

            return typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(content)
                .Invoke(null, [randomizer.Create(content, randomizer.Parent)]);
        }
        else
        {
            return Task.FromResult(randomizer.Create<int>());
        }
    }
}
