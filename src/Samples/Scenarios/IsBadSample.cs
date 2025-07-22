using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Samples.Scenarios;

public class IsBadSample : IIsGoodOrBadSample
{
    [ExcludeFromCodeCoverage]
    public int GoodOrBadProp
    {
        get => 0;
        set => throw new NotImplementedException();
    }
}
