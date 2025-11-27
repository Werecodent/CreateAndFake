using System.Collections.Frozen;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.RunnerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing randomization instances for <see cref="IRandomizer"/>.</summary>
public sealed class SelfCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly FrozenDictionary<Type, Func<IRandomizerChainer, object>> _Gens =
        new Dictionary<Type, Func<IRandomizerChainer, object>>()
        {
            { typeof(ToolSet), rand => ToolSet.CreateViaSeed(rand.Options.Gen.Next<int>()) },
            { typeof(SeededRandom), rand => new SeededRandom(rand.Options.Gen.Next<int>()) },
            { typeof(ValueRandom), rand => rand.Create<SeededRandom>() },
            { typeof(IRandomizer), rand => rand.Create<Randomizer>() },
            { typeof(IDuplicator), rand => rand.Create<Duplicator>() },
            { typeof(IExtractor), rand => rand.Create<Extractor>() },
            { typeof(IRandom), rand => rand.Create<SeededRandom>() },
            { typeof(IMutator), rand => rand.Create<Mutator>() },
            { typeof(IValuer), rand => rand.Create<Valuer>() },
            { typeof(ILimiter), rand => rand.Create<Limiter>() },
            { typeof(IAsyncLimiter), rand => rand.Create<Limiter>() },
            { typeof(ISyncLimiter), rand => rand.Create<Limiter>() },
            {
                typeof(Limiter),
                rand =>
                    rand.Options.Gen.NextItem([
                        Limiter.Once,
                        Limiter.Few,
                        Limiter.Dozen,
                        Limiter.Score,
                    ])
            },
            { typeof(AsserterMod), _ => (AsserterOptions mod) => mod },
            { typeof(DuplicatorMod), _ => (DuplicatorOptions mod) => mod },
            { typeof(ExtractorMod), _ => (ExtractorOptions mod) => mod },
            { typeof(FakerMod), _ => (FakerOptions mod) => mod },
            { typeof(MutatorMod), _ => (MutatorOptions mod) => mod },
            { typeof(RandomizerMod), _ => (RandomizerOptions mod) => mod },
            { typeof(RunnerMod), _ => (RunnerOptions mod) => mod },
            { typeof(TesterMod), _ => (TesterOptions mod) => mod },
            { typeof(ValuerMod), _ => (ValuerOptions mod) => mod },
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
