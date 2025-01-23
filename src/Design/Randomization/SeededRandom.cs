#if NET9_0_OR_GREATER
using Lock = System.Threading.Lock;
#else
using Lock = System.Object;
#endif

#pragma warning disable CA5394 // Do not use insecure randomness: Valid option and secure alternative provided.

namespace CreateAndFake.Design.Randomization;

/// <summary>For generating deterministic random values.</summary>
public sealed class SeededRandom : ValueRandom
{
    /// <summary>Lock to prevent thread collision with seeds.</summary>
    private readonly Lock _lock = new();

    /// <summary>Current seed to be used for the next randomized value.</summary>
    private int _seed;

    /// <inheritdoc cref="_seed"/>
    public int Seed
    {
        get { lock (_lock) { return _seed; } }
    }

    /// <inheritdoc/>
    public override int? InitialSeed { get; }

    /// <inheritdoc cref="SeededRandom(bool,int?)"/>
    public SeededRandom(int? seed = null) : this(true, seed) { }

    /// <inheritdoc cref="SeededRandom"/>
    /// <param name="onlyValidValues"><inheritdoc cref="ValueRandom.OnlyValidValues" path="/summary"/></param>
    /// <param name="seed"><inheritdoc cref="InitialSeed" path="/summary"/></param>
    public SeededRandom(bool onlyValidValues, int? seed = null) : base(onlyValidValues)
    {
        InitialSeed = seed ?? Environment.TickCount;
        _seed = InitialSeed.Value;
    }

    /// <inheritdoc/>
    protected override byte[] NextBytes(short length)
    {
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
}

#pragma warning restore CA5394 // Do not use insecure randomness
