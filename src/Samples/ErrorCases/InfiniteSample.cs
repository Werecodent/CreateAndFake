using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public sealed class InfiniteSample
{
    public InfiniteSample? Hole { get; set; }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
