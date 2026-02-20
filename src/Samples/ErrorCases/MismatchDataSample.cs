using CreateAndFake.Design.Content;

namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public class MismatchDataSample(int value)
{
    public string Data { get; set; } = "Value:" + value;

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
