using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public static class BaseUniqueAttributeTests
{
    [Fact]
    internal static Task BaseUniqueAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<BaseUniqueAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }

    [Fact]
    internal static Task BaseUniqueAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<BaseUniqueAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }
}
