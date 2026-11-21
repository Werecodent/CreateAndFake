using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample, ExcludeFromCreateAndFake]
public class MismatchDataSample(int value)
{
    public string Data { get; set; } = "Value:" + value;

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
