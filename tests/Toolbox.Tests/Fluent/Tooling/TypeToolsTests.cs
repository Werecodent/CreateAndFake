using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.Tests.Fluent.Tooling;

public static class TypeToolsTests
{
    [Fact]
    internal static Task TypeTools_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(object).Tools(),
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
            typeof(object).Tools(),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(ToolException), typeof(InvalidCastException)],
                }
        );
    }
}
