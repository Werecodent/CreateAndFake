using System.Collections.Frozen;

namespace CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class FloatValueHandler : ValueHandler<float>
{
    /// <summary>Abnormal float values to potentially exclude.</summary>
    private static readonly FrozenSet<float> _SpecialFloats = FrozenSet.Create(
        float.NaN,
        float.NegativeInfinity,
        float.PositiveInfinity
    );

    /// <inheritdoc/>
    protected override float Create(IRandom gen)
    {
        float value;
        do
        {
            value = BitConverter.ToSingle(gen.NextBytes(4), 0);
        } while (gen.OnlyValidValues && _SpecialFloats.Contains(value));
        return value;
    }

    /// <inheritdoc/>
    protected override float Create(IRandom gen, float min, float max)
    {
        return (float)(gen.NextPercent() * (max - min) + min);
    }
}
