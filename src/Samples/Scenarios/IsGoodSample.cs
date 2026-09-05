using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public class IsGoodSample : IIsGoodOrBadSample
{
    public int GoodOrBadProp { get; set; }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
