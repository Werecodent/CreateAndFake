using CreateAndFake.Samples.SingleValue;

namespace CreateAndFake.Samples.DoubleValue;

[ValidSample]
public abstract class BaseReadableHolder<T, TOther>(T value, TOther otherValue)
    : BaseReadableHolder<T>(value),
        IReadableHolder<T, TOther>
{
    public virtual TOther OtherValue { get; } = otherValue;

    public abstract TOther ReadOtherValue();
}
