using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Properties;

namespace CreateAndFake.Design.Randomization;

#pragma warning disable CA5394 // Secure alternative provided.

/// <summary>For generating deterministic random values.</summary>
public sealed class SeededRandom : ValueRandom, IDeepCloneable<SeededRandom>
{
    /// <summary>Prevents concurrency issues for <see cref="_seed"/>.</summary>
    private readonly Lock _lock = new();

    /// <inheritdoc cref="Seed"/>
    private int _seed;

    /// <summary>Current seed to be used for the next randomized value.</summary>
    public int Seed
    {
        get
        {
            lock (_lock)
            {
                return _seed;
            }
        }
    }

    /// <inheritdoc/>
    public override int? InitialSeed { get; }

    /// <inheritdoc cref="SeededRandom(int, bool,int?)"/>
    public SeededRandom(int? seed = null)
        : this(DesignDefaults.IterationLimit, !DesignDefaults.IncludeInfinityAndNaNGeneration, seed)
    { }

    /// <param name="seed"><inheritdoc cref="InitialSeed" path="/summary"/></param>
    /// <inheritdoc cref="SeededRandom(int, bool, int?, int)"/>
    public SeededRandom(int iterationLimit, bool onlyValidValues, int? seed = null)
        : base(iterationLimit, onlyValidValues)
    {
        InitialSeed = seed ?? Environment.TickCount;
        _seed = InitialSeed.Value;
    }

    /// <param name="initialSeed"><inheritdoc cref="InitialSeed" path="/summary"/></param>
    /// <param name="seed"><inheritdoc cref="_seed" path="/summary"/></param>
    /// <inheritdoc cref="SeededRandom"/>
    /// <inheritdoc cref="ValueRandom(int,bool)"/>
    private SeededRandom(int iterationLimit, bool onlyValidValues, int? initialSeed, int seed)
        : base(iterationLimit, onlyValidValues)
    {
        InitialSeed = initialSeed;
        _seed = seed;
    }

    /// <inheritdoc/>
    public override byte[] NextBytes(short length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "Cannot be negative.");
        }

        Random gen;
        lock (_lock)
        {
            gen = new Random(_seed);
            _seed = gen.Next();
        }

        byte[] buffer = new byte[length];
        gen.NextBytes(buffer);
        return buffer;
    }

    /// <inheritdoc/>
    public SeededRandom DeepClone()
    {
        return new SeededRandom(IterationLimit, OnlyValidValues, InitialSeed, Seed);
    }
}

#pragma warning restore CA5394
