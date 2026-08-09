using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.DoubleValue;

[ValidSample]
public abstract class BaseWriteableHolder<T, TOther>
    : BaseWriteableHolder<T>,
        IWriteableHolder<T, TOther>
{
    internal TOther? _otherValue = default;

    public virtual TOther OtherValue
    {
        set => _otherValue = value;
    }

    public abstract void WriteOtherValue(TOther otherValue);

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
