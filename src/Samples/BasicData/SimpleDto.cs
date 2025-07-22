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
}
