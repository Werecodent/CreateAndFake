using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="ICreateHandler"/> supported types for <see cref="IRandomizer"/>.</summary>
public sealed class HandlerCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.HandlerHint;

    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ICreateHandler[] _Creators =
    [
        new StringCreateHandler(),
        new ConfigurationSectionCreateHandler(),
    ];

    private static readonly IDictionary<Type, ICreateHandler[]> _CreatorsByType =
        TypeSupporter.GroupByInheritance(
            _Creators
                .Concat(SelfCreateHandlers.Handlers)
                .Concat(ValueCreateHandlers.Handlers)
                .Concat(SystemCreateHandlers.Handlers)
                .Concat(ExceptionCreateHandlers.Handlers)
                .Concat(ReflectionCreateHandlers.Handlers)
        );

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _CreatorsByType.Keys;

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (type != null && _CreatorsByType.TryGetValue(type, out ICreateHandler[]? creators))
        {
            return new(randomizer.Options.Gen.NextItem(creators).CreateSupported(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
