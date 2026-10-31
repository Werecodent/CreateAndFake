using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class GenericSampleTests
{
    [Theory, RandomData]
    public static async Task GenericSample_GuardsNulls([Fake] GenericSample<string> data)
    {
        data.ToString().SetupReturn(Behavior.Base<string>());

        await Tools.Tester.PreventsNullRefExceptionAsync(
            data,
            TestContext.Current.CancellationToken
        );

        data.Assert().Called();
    }

    [Fact]
    public static Task GenericSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(GenericSample<>),
            TestContext.Current.CancellationToken
        );
    }
}
