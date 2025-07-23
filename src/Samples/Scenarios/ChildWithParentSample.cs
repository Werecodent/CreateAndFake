namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ChildWithParentSample
{
    public ParentLoopSample? Parent { get; set; }

    public int Id { get; set; }
}
