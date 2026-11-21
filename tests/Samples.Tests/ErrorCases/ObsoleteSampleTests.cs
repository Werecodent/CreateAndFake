using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public sealed class ObsoleteSampleTests : AutoTester<ObsoleteSample>
{
    [Fact]
    public static Task ObsoleteSample_NoParameterMutationCanIncludeObsolete()
    {
        return Tools
            .Tester.PreventsParameterMutationAsync<ObsoleteSample>(
                TestContext.Current.CancellationToken,
                opt => opt with { IgnoreObsoleteMethods = false }
            )
            .Assert()
            .ThrowsAsync<InvalidOperationException>(TestContext.Current.CancellationToken);
    }
}
