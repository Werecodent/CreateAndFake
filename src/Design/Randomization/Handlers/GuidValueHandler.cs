using System.Globalization;
using System.Numerics;

namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class GuidValueHandler : ValueHandler<Guid>
{
    /// <inheritdoc/>
    protected override Guid Create(IRandom gen)
    {
        return new Guid(gen.NextBytes(16));
    }

    /// <inheritdoc/>
    protected override Guid Create(IRandom gen, Guid min, Guid max)
    {
        return ToGuid(gen.Next(ToBigInteger(min), ToBigInteger(max)));
    }

    /// <summary>Converts via <paramref name="value"/>'s hex representation.</summary>
    /// <param name="value">Instance to convert.</param>
    /// <returns>The converted <paramref name="value"/>.</returns>
    internal static BigInteger ToBigInteger(Guid value)
    {
        return BigInteger.Parse(
            "0" + value.ToString("N"),
            NumberStyles.AllowHexSpecifier,
            CultureInfo.InvariantCulture
        );
    }

    /// <summary>Converts via <paramref name="value"/>'s hex representation.</summary>
    /// <param name="value">Instance to convert.</param>
    /// <returns>The converted <paramref name="value"/>.</returns>
    internal static Guid ToGuid(BigInteger value)
    {
        return Guid.Parse(value.ToString("x33", CultureInfo.InvariantCulture).Substring(1));
    }
}
