using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public static class BaseFakeAttributeTests
{
    [Fact]
    internal static Task BaseFakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<BaseFakeAttribute>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(ToolException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static Task BaseFakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<BaseFakeAttribute>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(ToolException), typeof(ArgumentException)],
                }
        );
    }
}
