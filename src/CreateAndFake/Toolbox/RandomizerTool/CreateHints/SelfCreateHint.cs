using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing randomization instances for <see cref="IRandomizer"/>.</summary>
public sealed class SelfCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly Dictionary<Type, Func<RandomizerChainer, object>> _Gens = new()
        {
            { typeof(SeededRandom), rand => new SeededRandom(rand.Options.Gen.Next<int>()) },
            { typeof(ValueRandom), rand => rand.Create<SeededRandom>() },
            { typeof(IRandom), rand => rand.Create<SeededRandom>() },
            { typeof(ToolSet), rand => ToolSet.CreateViaSeed(rand.Options.Gen.Next<int>()) },
            { typeof(Limiter), rand => rand.Options.Gen.NextItem(
                [Limiter.Once, Limiter.Few, Limiter.Dozen, Limiter.Score, Limiter.Quick]) },
        };

    /// <inheritdoc/>
    protected internal override (bool, object?) TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type != null && _Gens.TryGetValue(type, out Func<RandomizerChainer, object?>? gen))
        {
            return (true, gen.Invoke(randomizer));
        }
        else
        {
            return (false, null);
        }
    }
}