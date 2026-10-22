using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.DoubleValue;

[ValidSample]
public abstract class BaseReadableHolder<T, TOther>(T value, TOther otherValue)
    : BaseReadableHolder<T>(value),
        IReadableHolder<T, TOther>
{
    public virtual TOther OtherValue { get; } = otherValue;

    public abstract TOther ReadOtherValue();

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
