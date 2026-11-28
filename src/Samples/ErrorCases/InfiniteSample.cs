using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample, ExcludeFromCreateAndFake]
public sealed class InfiniteSample
{
    public InfiniteSample? Hole { get; set; }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
