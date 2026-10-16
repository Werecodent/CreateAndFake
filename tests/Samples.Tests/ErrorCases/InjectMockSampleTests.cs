using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public static class InjectMockSampleTests
{
    [Theory, RandomData]
    public static void InjectMockSample_CanBeInjected([Inject] InjectMockSample sample)
    {
        sample.TestIfMockedSeparately();
    }
}
