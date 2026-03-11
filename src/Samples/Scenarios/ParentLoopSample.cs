using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ParentLoopSample
{
    public ChildWithParentSample? Child { get; set; }

    public int Id { get; set; }

    public override string ToString()
    {
        return TypeHelper.ExpandedName(GetType());
    }
}
