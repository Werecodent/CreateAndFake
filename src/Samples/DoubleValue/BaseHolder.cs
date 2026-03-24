using CreateAndFake.Design.Types;
using CreateAndFake.Samples.SingleValue;

namespace CreateAndFake.Samples.DoubleValue;

[ValidSample]
public abstract class BaseHolder<T, TOther>(T value, TOther otherValue)
    : Holder<T>(value),
        IHolder<T, TOther>
{
    public virtual TOther OtherValue { get; set; } = otherValue;

    public abstract TOther ReadOtherValue();

    public abstract void WriteOtherValue(TOther otherValue);

    public override string ToString()
    {
        return GenericTypeConverter.ExpandedName(GetType());
    }
}
