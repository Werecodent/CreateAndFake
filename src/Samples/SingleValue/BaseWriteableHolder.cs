namespace CreateAndFake.Samples.SingleValue;

[ValidSample]
public abstract class BaseWriteableHolder<T> : IWriteableHolder<T>
{
    internal T? _value = default;

    public virtual T Value
    {
        set => _value = value;
    }

    public abstract void WriteValue(T value);
}
