using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public class IsGoodSample : IIsGoodOrBadSample
{
    public int GoodOrBadProp { get; set; }

    public override string ToString()
    {
        return TypeHelper.ExpandedName(GetType());
    }
}
