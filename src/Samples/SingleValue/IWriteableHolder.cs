namespace Werecodent.CreateAndFake.Samples.SingleValue;

[ValidSample]
public interface IWriteableHolder<in T>
{
    T Value { set; }

    void WriteValue(T value);
}
