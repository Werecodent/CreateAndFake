using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing async collection instances for <see cref="IRandomizer"/>.</summary>
public sealed class NullableCreateHint : CreateHint
{
    /// <summary>Method used to create a <see cref="Nullable"/>.</summary>
    /// <remarks>Dynamic can't be used due to creating the wrong type with subclasses.</remarks>
    private static readonly MethodInfo _NullableMaker = typeof(NullableCreateHint).GetMethod(
        nameof(MakeNullableResult),
        BindingFlags.NonPublic | BindingFlags.Static
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.NullableHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(Nullable<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? asGeneric = GenericConverter.AsGenericBase(type);
        if (asGeneric == typeof(Nullable<>))
        {
            Type contentType = type.GetGenericArguments().Single();
            object content = randomizer.Create(contentType);
            return new(_NullableMaker.MakeGenericMethod(contentType).Invoke(null, [content]));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    private static T? MakeNullableResult<T>(T item)
        where T : struct
    {
        return new T?(item);
    }
}
