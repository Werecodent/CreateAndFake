namespace Werecodent.CreateAndFake.Samples.BasicData;

/// <summary>Inherits and references a child class.</summary>
[ValidSample]
public class RecursiveDto : SimpleDto
{
    public SimpleDto? SimpleValue { get; set; }
}
