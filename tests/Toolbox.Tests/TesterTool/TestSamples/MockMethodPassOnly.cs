using CreateAndFake.Samples.ErrorCases;

namespace CreateAndFake.Tests.TesterTool.TestSamples;

#pragma warning disable // For testing.

public sealed class MockMethodPassOnly
{
    public bool MockPassOnly(IOnlyMockSample sample)
    {
        sample?.FailIfNotMocked();
        return true;
    }
}

#pragma warning restore
