namespace Werecodent.CreateAndFake.Samples.SingleValue;

[ValidSample]
public class WriteableHolder<T> : BaseWriteableHolder<T>
{
    public override void WriteValue(T value)
    {
        Value = value;
    }
}
