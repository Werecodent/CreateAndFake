namespace Werecodent.CreateAndFake.Samples.DoubleValue;

[ValidSample]
public class ReadableHolder<T, TOther>(T value, TOther otherValue)
    : BaseReadableHolder<T, TOther>(value, otherValue)
{
    public override T ReadValue()
    {
        return Value;
    }

    public override TOther ReadOtherValue()
    {
        return OtherValue;
    }
}
