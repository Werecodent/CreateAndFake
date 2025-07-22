namespace CreateAndFake.Samples.BasicData;

/// <summary>Combination of all basic data sample designs.</summary>
[ValidSample]
public class CompleteDto : SimpleDto
{
    public SimpleDto? SimpleValue { get; set; }

    public CollectionDto? CollectionsValue { get; set; }
}
