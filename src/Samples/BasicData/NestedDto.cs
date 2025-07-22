namespace CreateAndFake.Samples.BasicData;

/// <summary>Holds a reference to another basic data class.</summary>
[ValidSample]
public class NestedDto
{
    public SimpleDto? SimpleValue { get; set; }
}
