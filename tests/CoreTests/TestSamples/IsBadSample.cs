using System.Diagnostics.CodeAnalysis;

namespace CreateAndFakeTests.TestSamples;

public class IsBadSample : IIsGoodOrBadSample
{
    [ExcludeFromCodeCoverage]
    public int GoodOrBadProp
    {
        get => 0;
        set => throw new NotImplementedException();
    }
}
