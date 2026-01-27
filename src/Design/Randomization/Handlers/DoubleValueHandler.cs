using System.Collections.Frozen;

namespace CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class DoubleValueHandler : ValueHandler<double>
{
    /// <summary>Abnormal double values to potentially exclude.</summary>
    private static readonly FrozenSet<double> _SpecialDoubles = FrozenSet.Create(
        double.NaN,
        double.NegativeInfinity,
        double.PositiveInfinity
    );

    /// <inheritdoc/>
    protected override double Create(IRandom gen)
    {
        double value;
        do
        {
            value = BitConverter.ToDouble(gen.NextBytes(8), 0);
        } while (gen.OnlyValidValues && _SpecialDoubles.Contains(value));
        return value;
    }

    /// <inheritdoc/>
    protected override double Create(IRandom gen, double min, double max)
    {
        double percent = gen.NextPercent();
        return max * percent + min * (1 - percent);
    }
}
