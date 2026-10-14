using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public static class BaseCopyAttributeTests
{
    [Fact]
    internal static Task BaseCopyAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<BaseCopyAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task BaseCopyAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<BaseCopyAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }

    [Fact]
    internal static Task CreateParameterValue_PreventsNonValue()
    {
        return Tools
            .Runner.CreateForAsync(
                typeof(BaseCopyAttributeTests).GetMethod(
                    nameof(BadNothingToCopy),
                    BindingFlags.Static | BindingFlags.NonPublic
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    private static DataSample BadNothingToCopy([Copy] DataSample sample)
    {
        return sample;
    }
}
