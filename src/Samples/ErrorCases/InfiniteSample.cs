using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public sealed class InfiniteSample
{
    public InfiniteSample? Hole { get; set; }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
