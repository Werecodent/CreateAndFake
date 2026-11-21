using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.BasicData;

/// <summary>Holds a reference to another basic data class.</summary>
[ValidSample]
public class NestedDto
{
    public SimpleDto? SimpleValue { get; set; }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
