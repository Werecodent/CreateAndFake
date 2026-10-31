using System.Reflection;
using System.Threading.Tasks.Sources;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="ValueTask{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class ValueTaskCreateHint : CreateHint
{
    /// <summary>Method used to create a completed <see cref="IValueTaskSource"/>.</summary>
    /// <remarks>Dynamic can't be used due to creating the wrong type with subclasses.</remarks>
    private static readonly MethodInfo _ValueSourceMaker = typeof(ValueTaskCreateHint).GetMethod(
        nameof(CreateSource),
        BindingFlags.NonPublic | BindingFlags.Static
    )!;

    /// <summary>Method used to create a completed <see cref="ValueTask"/>.</summary>
    /// <remarks>Dynamic can't be used due to creating the wrong type with subclasses.</remarks>
    private static readonly MethodInfo _ValueTaskMaker = typeof(ValueTaskCreateHint).GetMethod(
        nameof(CreateValueTaskAsync),
        BindingFlags.NonPublic | BindingFlags.Static
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.TaskHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(ValueTask<>), typeof(IValueTaskSource<>), typeof(SingleCallValueTaskSource<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = GenericConverter.AsGenericBase(type);

        if (
            asGeneric == typeof(SingleCallValueTaskSource<>)
            || asGeneric == typeof(IValueTaskSource<>)
        )
        {
            Type contentType = type.GetGenericArguments().Single();
            object content = randomizer.Create(contentType);
            return new(_ValueSourceMaker.MakeGenericMethod(contentType).Invoke(null, [content]));
        }
        else if (asGeneric == typeof(ValueTask<>))
        {
            Type contentType = type.GetGenericArguments().Single();
            return new(
                _ValueTaskMaker
                    .MakeGenericMethod(contentType)
                    .Invoke(null, [randomizer.Create(contentType), randomizer])
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    private static ValueTask<T> CreateValueTaskAsync<T>(T content, IRandomizerChainer randomizer)
    {
        return new ValueTask<T>(CreateSource(content), randomizer.Create<short>());
    }

    private static SingleCallValueTaskSource<T> CreateSource<T>(T content)
    {
        return new SingleCallValueTaskSource<T>(content);
    }
}
