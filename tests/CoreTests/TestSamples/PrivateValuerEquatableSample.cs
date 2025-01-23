using CreateAndFake.Design;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFakeTests.TestSamples;

public class PrivateValuerEquatableSample(string stringValue) : IValuerEquatable
{
    private string StringValue { get; } = stringValue;

    public virtual bool ValuesEqual(object other, IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return (other is PrivateValuerEquatableSample sample)
            && valuer.Equals(StringValue, sample.StringValue);
    }

    public virtual int GetValueHash(IValuer valuer)
    {
        return valuer?.GetHashCode(StringValue) ?? throw new ArgumentNullException(nameof(valuer));
    }
}
