using CreateAndFake.Design.Content;

namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public sealed class InfiniteSample
{
    public InfiniteSample? Hole { get; set; }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
