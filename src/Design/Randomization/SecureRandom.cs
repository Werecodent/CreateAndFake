using System.Security.Cryptography;

namespace CreateAndFake.Design.Randomization;

/// <summary>For slowly generating cryptographically secure random values.</summary>
/// <inheritdoc/>
public sealed class SecureRandom(bool onlyValidValues = true) : ValueRandom(onlyValidValues)
{
    /// <inheritdoc/>
    public override int? InitialSeed { get; } = null;

    /// <summary>Source generator used for random bytes.</summary>
    private static readonly RandomNumberGenerator _Gen = RandomNumberGenerator.Create();

    /// <inheritdoc/>
    protected override byte[] NextBytes(short length)
    {
        byte[] buffer = new byte[length];
        _Gen.GetBytes(buffer);
        return buffer;
    }
}
