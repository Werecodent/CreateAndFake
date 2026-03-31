using CreateAndFake.Design.Exceptions;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerChainerTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(UnsupportedException),
                typeof(ToolException),
                typeof(EngineException),
            ],
        };

    [Fact]
    internal static Task ValuerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<IValuerChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task ValuerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<IValuerChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }
}
