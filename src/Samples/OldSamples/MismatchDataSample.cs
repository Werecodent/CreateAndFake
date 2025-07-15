namespace CreateAndFake.Samples.OldSamples;

public class MismatchDataSample(int value)
{
    public string Data { get; set; } = "Value:" + value;
}
