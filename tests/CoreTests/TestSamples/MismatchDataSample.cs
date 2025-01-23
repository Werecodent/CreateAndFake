namespace CreateAndFakeTests.TestSamples;

public class MismatchDataSample(int value)
{
    public string Data { get; set; } = "Value:" + value;
}
