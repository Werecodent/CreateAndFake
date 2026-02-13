using CreateAndFake.Design.Content;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ParentLoopSample
{
    public ChildWithParentSample? Child { get; set; }

    public int Id { get; set; }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
