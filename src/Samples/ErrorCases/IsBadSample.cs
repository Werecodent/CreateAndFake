using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Design.Types;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public class IsBadSample : IIsGoodOrBadSample
{
    [ExcludeFromCodeCoverage]
    public int GoodOrBadProp
    {
        get => 0;
        set => throw new NotImplementedException();
    }

    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
