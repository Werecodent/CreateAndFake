namespace CreateAndFake.Samples.SingleValue;

[ValidSample]
public abstract class BaseReadableHolder<T>(T value) : IReadableHolder<T>
{
    public virtual T Value { get; } = value;

    public abstract T ReadValue();
}
