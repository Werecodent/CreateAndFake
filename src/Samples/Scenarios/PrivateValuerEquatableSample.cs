using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public class PrivateValuerEquatableSample(string stringValue) : IValuerEquatable
{
    private string StringValue { get; } = stringValue;

    public virtual bool ValuesEqual(object? other, IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return (other is PrivateValuerEquatableSample sample)
            && valuer.Equals(StringValue, sample.StringValue);
    }

    public virtual int GetValueHash(IValuer valuer)
    {
        return valuer?.GetHashCode(StringValue) ?? throw new ArgumentNullException(nameof(valuer));
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
