using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.BasicData;

/// <summary>Assorted basic values.</summary>
[ValidSample]
public class SimpleDto
{
    public int IntValue { get; set; }

    public double DoubleValue { get; set; }

    public string? StringValue { get; set; }

    public object? ObjectValue { get; set; }

    public DateTime DateValue { get; set; }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
