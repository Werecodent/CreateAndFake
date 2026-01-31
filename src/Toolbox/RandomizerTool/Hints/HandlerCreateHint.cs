using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="ICreateHandler"/> supported types for <see cref="IRandomizer"/>.</summary>
public sealed class HandlerCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ICreateHandler[] _Creators =
    [
        new StringCreateHandler(),
        new ConfigurationSectionCreateHandler(),
    ];

    private static readonly IDictionary<Type, ICreateHandler[]> _CreatorsByType =
        TypeSupporter.GroupByInheritance(
            _Creators
                .Concat(ValueCreateHandlers.Handlers)
                .Concat(ExceptionCreateHandlers.Handlers)
                .Concat(ReflectionCreateHandlers.Handlers)
                .Concat(SelfCreateHandlers.Handlers)
                .Concat(SystemCreateHandlers.Handlers)
        );

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _CreatorsByType.Keys;

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (
            type != null
            && type != typeof(object)
            && _CreatorsByType.TryGetValue(type, out ICreateHandler[]? creators)
        )
        {
            return new(randomizer.Options.Gen.NextItem(creators).CreateSupported(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
