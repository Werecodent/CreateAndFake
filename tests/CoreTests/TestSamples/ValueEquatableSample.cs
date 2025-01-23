using CreateAndFake.Design.Content;

namespace CreateAndFakeTests.TestSamples;

public class ValueEquatableSample : IValueEquatable
{
    public string StringValue { get; set; }

    public int NumberValue;

    public virtual bool ValuesEqual(object other)
    {
        return (other is ValueEquatableSample sample)
            && StringValue == sample.StringValue
            && NumberValue == sample.NumberValue;
    }

    public virtual int GetValueHash()
    {
        return ValueComparer.Use.GetHashCode(StringValue, NumberValue);
    }
}
