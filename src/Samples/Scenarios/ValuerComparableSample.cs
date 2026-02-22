using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public class ValuerComparableSample : IValuerComparable
{
    public string? StringValue { get; set; }

    public int NumberValue;

    public virtual IEnumerable<Difference> Compare(object? other, IValuer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        if (other is ValuerComparableSample sample)
        {
            foreach (Difference diff in valuer.Compare(StringValue, sample.StringValue))
            {
                yield return new Difference($".{nameof(StringValue)}", diff);
            }
            foreach (Difference diff in valuer.Compare(NumberValue, sample.NumberValue))
            {
                yield return new Difference($".{nameof(NumberValue)}", diff);
            }
        }
        else
        {
            yield return new Difference(GetType(), other?.GetType());
        }
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
