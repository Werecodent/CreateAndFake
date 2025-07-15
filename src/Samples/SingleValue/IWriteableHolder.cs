namespace CreateAndFake.Samples.SingleValue;

public interface IWriteableHolder<in T>
{
    T Value { set; }

    void WriteValue(T value);
}
