using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Randomization;

#pragma warning disable CA5394 // Secure alternative provided.

/// <summary>For generating deterministic random values.</summary>
public sealed class SeededRandom : ValueRandom, IDeepCloneable
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

    /// <inheritdoc cref="SeededRandom(bool,int?)"/>
    public SeededRandom(int? seed = null)
        : this(true, seed) { }

    /// <param name="seed"><inheritdoc cref="InitialSeed" path="/summary"/></param>
    /// <inheritdoc cref="SeededRandom(bool, int?, int)"/>
    public SeededRandom(bool onlyValidValues, int? seed = null)
        : base(onlyValidValues)
    {
        InitialSeed = seed ?? Environment.TickCount;
        _seed = InitialSeed.Value;
    }

    /// <param name="initialSeed"><inheritdoc cref="InitialSeed" path="/summary"/></param>
    /// <param name="seed"><inheritdoc cref="_seed" path="/summary"/></param>
    /// <inheritdoc cref="SeededRandom"/>
    /// <inheritdoc cref="ValueRandom(bool)"/>
    private SeededRandom(bool onlyValidValues, int? initialSeed, int seed)
        : base(onlyValidValues)
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
    public IDeepCloneable DeepClone()
    {
        return new SeededRandom(OnlyValidValues, InitialSeed, Seed);
    }
}

#pragma warning restore CA5394
