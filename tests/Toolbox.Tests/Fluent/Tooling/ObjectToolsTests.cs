using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent.Tooling;

namespace Werecodent.CreateAndFake.Tests.Fluent.Tooling;

public static class ObjectToolsTests
{
    [Fact]
    internal static Task ObjectTools_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ObjectTools<IFaked>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }

    [Fact]
    internal static Task ObjectTools_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ObjectTools<IFaked>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ToolException)] }
        );
    }
}
