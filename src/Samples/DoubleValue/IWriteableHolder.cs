using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.DoubleValue;

[ValidSample]
public interface IWriteableHolder<in T, in TOther> : IWriteableHolder<T>
{
    TOther OtherValue { set; }

    void WriteOtherValue(TOther otherValue);
}
