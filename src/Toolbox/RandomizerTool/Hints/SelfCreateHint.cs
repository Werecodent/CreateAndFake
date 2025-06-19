using System.Collections.Frozen;
using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing randomization instances for <see cref="IRandomizer"/>.</summary>
public sealed class SelfCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly FrozenDictionary<Type, Func<IRandomizerChainer, object>> _Gens =
        new Dictionary<Type, Func<IRandomizerChainer, object>>()
        {
            { typeof(SeededRandom), rand => new SeededRandom(rand.Options.Gen.Next<int>()) },
            { typeof(ValueRandom), rand => rand.Create<SeededRandom>() },
            { typeof(IRandom), rand => rand.Create<SeededRandom>() },
            { typeof(IRandomizer), rand => rand.Create<Randomizer>() },
            { typeof(IDuplicator), rand => rand.Create<Duplicator>() },
            { typeof(IMutator), rand => rand.Create<Mutator>() },
            { typeof(IExtractor), rand => rand.Create<Extractor>() },
            { typeof(IValuer), rand => rand.Create<Valuer>() },
            { typeof(ToolSet), rand => ToolSet.CreateViaSeed(rand.Options.Gen.Next<int>()) },
            {
                typeof(Limiter),
                rand =>
                    rand.Options.Gen.NextItem(
                        [Limiter.Once, Limiter.Few, Limiter.Dozen, Limiter.Score]
                    )
            },
        }.ToFrozenDictionary();

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type != null && _Gens.TryGetValue(type, out Func<IRandomizerChainer, object?>? gen))
        {
            return new(gen.Invoke(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
