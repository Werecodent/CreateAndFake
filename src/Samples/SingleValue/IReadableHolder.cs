namespace CreateAndFake.Samples.SingleValue;

[ValidSample]
public interface IReadableHolder<out T>
{
    T Value { get; }

    T ReadValue();
}
