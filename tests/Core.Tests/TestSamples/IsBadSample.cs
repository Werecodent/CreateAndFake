using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Tests.TestSamples;

public class IsBadSample : IIsGoodOrBadSample
{
    [ExcludeFromCodeCoverage]
    public int GoodOrBadProp
    {
        get => 0;
        set => throw new NotImplementedException();
    }
}
