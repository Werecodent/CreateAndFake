using CreateAndFake.Design.Content;

namespace CreateAndFake.Samples.BasicData;

/// <summary>Holds a reference to another basic data class.</summary>
[ValidSample]
public class NestedDto
{
    public SimpleDto? SimpleValue { get; set; }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
