using System.Numerics;

namespace CreateAndFake.Design.Randomization.Handlers;

#pragma warning disable IDE0056 // Indexing not available.

/// <inheritdoc/>
internal sealed class BigIntegerValueHandler : ValueHandler<BigInteger>
{
    /// <inheritdoc/>
    protected override BigInteger Create(IRandom gen)
    {
        return new BigInteger(gen.NextBytes(8));
    }

    /// <inheritdoc/>
    protected override BigInteger Create(IRandom gen, BigInteger min, BigInteger max)
    {
        BigInteger range = max - min;
        byte[] rangeBytes = range.ToByteArray();

        byte zeroBitsMask = 0b00000000;
        byte mostSignificantByte = rangeBytes[rangeBytes.Length - 1];

        for (int i = 7; i >= 0; i--) // Sign bit 8 is always 0.
        {
            if ((mostSignificantByte & (0b1 << i)) != 0)
            {
                zeroBitsMask = (byte)(0b11111111 >> (7 - i));
                break;
            }
        }

        BigInteger result;
        do
        {
            byte[] bytes = gen.NextBytes((short)rangeBytes.Length);
            bytes[bytes.Length - 1] &= zeroBitsMask;

            result = new BigInteger(bytes);
        } while (result > range);

        return result + min;
    }
}

#pragma warning restore
