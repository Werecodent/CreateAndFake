using CreateAndFake.Design.Content;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public class IsGoodSample : IIsGoodOrBadSample
{
    public int GoodOrBadProp { get; set; }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
