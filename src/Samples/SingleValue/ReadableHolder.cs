namespace Werecodent.CreateAndFake.Samples.SingleValue;

[ValidSample]
public class ReadableHolder<T>(T value) : BaseReadableHolder<T>(value)
{
    public override T ReadValue()
    {
        return Value;
    }
}
