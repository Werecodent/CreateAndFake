namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public class MismatchDataSample(int value)
{
    public string Data { get; set; } = "Value:" + value;
}
