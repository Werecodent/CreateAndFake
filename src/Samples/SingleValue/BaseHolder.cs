using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.SingleValue;

[ValidSample]
public abstract class BaseHolder<T>(T value) : IReadableHolder<T>, IWriteableHolder<T>
{
    public virtual T Value { get; set; } = value;

    public abstract T ReadValue();

    public abstract void WriteValue(T value);

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
