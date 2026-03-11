using CreateAndFake.Design.Types;
using CreateAndFake.Samples.SingleValue;

namespace CreateAndFake.Samples.DoubleValue;

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
        return TypeHelper.ExpandedName(GetType());
    }
}
