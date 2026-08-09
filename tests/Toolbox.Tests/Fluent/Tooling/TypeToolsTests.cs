using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Fluent.Tooling;

namespace Werecodent.CreateAndFake.Tests.Fluent.Tooling;

public static class TypeToolsTests
{
    [Fact]
    internal static Task TypeTools_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TypeTools),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(ToolException), typeof(InvalidCastException)],
                }
        );
    }

    [Fact]
    internal static Task TypeTools_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TypeTools),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(ToolException), typeof(InvalidCastException)],
                }
        );
    }
}
