namespace CreateAndFake.Samples.SingleValue;

public interface IReadableHolder<out T>
{
    T Value { get; }

    T ReadValue();
}
