namespace CreateAndFakeTests.TestSamples;

public class FieldSample(string stringValue, string stringValue2)
{
    public const int Perm = 1;

    public readonly string PermText = stringValue2;

    public string StringValue = stringValue;

    public int NumberValue;

    public IEnumerable<string> CollectionValue;
}
