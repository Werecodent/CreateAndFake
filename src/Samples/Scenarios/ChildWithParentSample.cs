using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ChildWithParentSample
{
    public ParentLoopSample? Parent { get; set; }

    public int Id { get; set; }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
