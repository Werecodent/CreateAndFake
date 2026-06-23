using System.Threading.Tasks.Sources;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="ValueTask{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class ValueTaskCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.TaskHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(ValueTask<>), typeof(IValueTaskSource<>), typeof(SingleCallValueTaskSource<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = GenericTypeConverter.AsGenericBase(type);

        if (
            asGeneric == typeof(SingleCallValueTaskSource<>)
            || asGeneric == typeof(IValueTaskSource<>)
        )
        {
            object content = randomizer.Create(type.GetGenericArguments().Single());
            return new(CreateSource((dynamic)content));
        }
        else if (asGeneric == typeof(ValueTask<>))
        {
            object content = randomizer.Create(type.GetGenericArguments().Single());
            return new(CreateValueTask((dynamic)content, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    private static ValueTask<T> CreateValueTask<T>(T content, IRandomizerChainer randomizer)
    {
        return new ValueTask<T>(CreateSource(content), randomizer.Create<short>());
    }

    private static SingleCallValueTaskSource<T> CreateSource<T>(T content)
    {
        return new SingleCallValueTaskSource<T>(content);
    }
}
