namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class ParentLoopSample
{
    public ChildWithParentSample? Child { get; set; }

    public int Id { get; set; }
}
