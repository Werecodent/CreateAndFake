namespace Werecodent.CreateAndFake.Samples.SingleValue;

[ValidSample]
public class Holder<T>(T value) : BaseHolder<T>(value)
{
    public override T ReadValue()
    {
        return Value;
    }

    public override void WriteValue(T value)
    {
        Value = value;
    }
}
