using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public class ValuerEquatableSample : IValuerEquatable
{
    public string? StringValue { get; set; }

    public int NumberValue;

    public virtual bool ValuesEqual(object? other, IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return (other is ValuerEquatableSample sample)
            && valuer.Equals(StringValue, sample.StringValue)
            && valuer.Equals(NumberValue, sample.NumberValue);
    }

    public virtual int GetValueHash(IValuer valuer)
    {
        return valuer?.GetHashCode(new object?[] { StringValue, NumberValue })
            ?? throw new ArgumentNullException(nameof(valuer));
    }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
