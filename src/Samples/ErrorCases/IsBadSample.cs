using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

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
        return GenericConverter.ExpandName(GetType());
    }
}
