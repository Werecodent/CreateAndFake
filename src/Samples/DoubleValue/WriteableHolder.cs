namespace Werecodent.CreateAndFake.Samples.DoubleValue;

[ValidSample]
public class WriteableHolder<T, TOther> : BaseWriteableHolder<T, TOther>
{
    public override void WriteValue(T value)
    {
        Value = value;
    }

    public override void WriteOtherValue(TOther otherValue)
    {
        OtherValue = otherValue;
    }
}
