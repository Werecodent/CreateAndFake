using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class ConstraintSampleTests
{
    [Theory, RandomData]
    public static async Task ConstraintSample_GuardsNulls(
        [Fake] ConstraintSample<int, DataSample> data
    )
    {
        data.ToString().SetupReturn(Behavior.Base<string>());

        await Tools.Tester.PreventsNullRefExceptionAsync(
            data,
            TestContext.Current.CancellationToken
        );

        data.Assert().Called();
    }

    [Fact]
    public static Task ConstraintSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ConstraintSample<,>),
            TestContext.Current.CancellationToken
        );
    }
}
