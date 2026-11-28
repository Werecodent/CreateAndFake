using Werecodent.CreateAndFake.Design.Properties;

namespace Werecodent.CreateAndFake.Design.Randomization;

#pragma warning disable CA5394 // Secure alternative provided.

/// <summary>For quickly generating cryptographically insecure random values.</summary>
/// <inheritdoc/>
public sealed class FastRandom(
    int iterationLimit = DesignDefaults.IterationLimit,
    bool onlyValidValues = !DesignDefaults.IncludeInfinityAndNaNGeneration
) : ValueRandom(iterationLimit, onlyValidValues)
{
    /// <summary>Prevents concurrency issues for <see cref="_Gen"/>.</summary>
    private static readonly Lock _Lock = new();

    /// <summary>Source generator used for random <see cref="byte"/>s.</summary>
    private static readonly Random _Gen = new();

    /// <inheritdoc/>
    public override int? InitialSeed { get; } = null;

    /// <inheritdoc/>
    public override byte[] NextBytes(short length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "Cannot be negative.");
        }

        byte[] buffer = new byte[length];
        lock (_Lock)
        {
            _Gen.NextBytes(buffer);
        }
        return buffer;
    }
}

#pragma warning restore
