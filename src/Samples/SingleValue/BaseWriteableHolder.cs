using CreateAndFake.Design.Types;

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

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
