using CreateAndFake.Samples.SingleValue;

namespace CreateAndFake.Samples.DoubleValue;

[ValidSample]
public class Holder<T, TOther>(T value, TOther otherValue)
    : BaseHolder<T, TOther>(value, otherValue)
{
    public override TOther ReadOtherValue()
    {
        return OtherValue;
    }

    public override void WriteOtherValue(TOther otherValue)
    {
        OtherValue = otherValue;
    }
}
