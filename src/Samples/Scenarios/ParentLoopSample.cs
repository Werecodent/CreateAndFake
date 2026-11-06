using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ParentLoopSample
{
    public ChildWithParentSample? Child { get; set; }

    public int Id { get; set; }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
