namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public class DataSample
{
    public string? StringValue { get; set; }

    public int NumberValue { get; set; }

    public IEnumerable<string?>? CollectionValue { get; set; }
}
