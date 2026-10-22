using System.Security.Cryptography;
using Werecodent.CreateAndFake.Design.Properties;

namespace Werecodent.CreateAndFake.Design.Randomization;

/// <summary>For slowly generating cryptographically secure random values.</summary>
/// <inheritdoc/>
public sealed class SecureRandom(
    int iterationLimit = DesignDefaults.IterationLimit,
    bool onlyValidValues = !DesignDefaults.IncludeInfinityAndNaNGeneration
) : ValueRandom(iterationLimit, onlyValidValues)
{
    /// <inheritdoc/>
    public override int? InitialSeed { get; } = null;

    /// <summary>Source generator used for random bytes.</summary>
    private static readonly RandomNumberGenerator _Gen = RandomNumberGenerator.Create();

    /// <inheritdoc/>
    public override byte[] NextBytes(short length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "Cannot be negative.");
        }

        byte[] buffer = new byte[length];
        _Gen.GetBytes(buffer);
        return buffer;
    }
}
