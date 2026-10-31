using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public static class BaseInjectAttributeTests
{
    [Fact]
    internal static Task BaseInjectAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<BaseInjectAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }

    [Fact]
    internal static Task BaseInjectAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<BaseInjectAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }
}
