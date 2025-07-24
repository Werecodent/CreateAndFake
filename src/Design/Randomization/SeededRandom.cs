using CreateAndFake.Design.Content;
#if NET9_0_OR_GREATER
using Lock = System.Threading.Lock;
#else
using Lock = System.Object;
#endif

namespace CreateAndFake.Design.Randomization;

#pragma warning disable CA5394 // Secure alternative provided.

/// <summary>For generating deterministic random values.</summary>
public sealed class SeededRandom : ValueRandom, IDeepCloneable
{
    /// <summary>Lock to prevent thread collision with seeds.</summary>
    private readonly Lock _lock = new();

    /// <summary>Current seed to be used for the next randomized value.</summary>
    private int _seed;

    /// <inheritdoc cref="_seed"/>
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

    /// <inheritdoc cref="SeededRandom"/>
    /// <param name="onlyValidValues">
    ///     <inheritdoc cref="ValueRandom.OnlyValidValues" path="/summary"/>
    /// </param>
    /// <param name="initialSeed"><inheritdoc cref="InitialSeed" path="/summary"/></param>
    /// <param name="seed"><inheritdoc cref="_seed" path="/summary"/></param>
    private SeededRandom(bool onlyValidValues, int? initialSeed, int seed)
        : base(onlyValidValues)
    {
        InitialSeed = initialSeed;
        _seed = seed;
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

    /// <inheritdoc/>
    public IDeepCloneable DeepClone()
    {
        return new SeededRandom(OnlyValidValues, InitialSeed, _seed);
    }
}

#pragma warning restore CA5394
