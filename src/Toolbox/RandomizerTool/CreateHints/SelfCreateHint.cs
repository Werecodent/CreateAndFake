using System.Collections.Frozen;
using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.RandomizerTool.CreateHints;

/// <summary>Handles randomizing randomization instances for <see cref="IRandomizer"/>.</summary>
public sealed class SelfCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly FrozenDictionary<Type, Func<RandomizerChainer, object>> _Gens =
        new Dictionary<Type, Func<RandomizerChainer, object>>()
        {
            { typeof(SeededRandom), rand => new SeededRandom(rand.Options.Gen.Next<int>()) },
            { typeof(ValueRandom), rand => rand.Create<SeededRandom>() },
            { typeof(IRandom), rand => rand.Create<SeededRandom>() },
            { typeof(ToolSet), rand => ToolSet.CreateViaSeed(rand.Options.Gen.Next<int>()) },
            {
                typeof(Limiter),
                rand =>
                    rand.Options.Gen.NextItem(
                        [Limiter.Once, Limiter.Few, Limiter.Dozen, Limiter.Score]
                    )
            },
            {
                typeof(AsyncLimiter),
                rand =>
                    rand.Options.Gen.NextItem(
                        [
                            AsyncLimiter.Once,
                            AsyncLimiter.Few,
                            AsyncLimiter.Dozen,
                            AsyncLimiter.Score,
                            AsyncLimiter.Quick,
                        ]
                    )
            },
        }.ToFrozenDictionary();

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type != null && _Gens.TryGetValue(type, out Func<RandomizerChainer, object?>? gen))
        {
            return new(gen.Invoke(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
