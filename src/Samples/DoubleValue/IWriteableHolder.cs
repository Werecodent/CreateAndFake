using CreateAndFake.Samples.SingleValue;

namespace CreateAndFake.Samples.DoubleValue;

[ValidSample]
public interface IWriteableHolder<in T, in TOther> : IWriteableHolder<T>
{
    TOther OtherValue { set; }

    void WriteOtherValue(TOther otherValue);
}
