using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

#pragma warning disable CA1859 // False positive due to dynamic.

/// <summary>Handles randomizing <see cref="Task{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class TaskCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.TaskHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(Task), typeof(Task<>), typeof(TaskCompletionSource<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = TypeHelper.AsGenericBase(type);

        if (asGeneric == typeof(Task<>) || asGeneric == typeof(TaskCompletionSource<>))
        {
            Type content = type.GetGenericArguments().Single();
            return new(Task.FromResult((dynamic)randomizer.Create(content)));
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
}

#pragma warning restore CA1859
