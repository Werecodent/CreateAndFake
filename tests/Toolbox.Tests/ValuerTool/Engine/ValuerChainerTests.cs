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
                typeof(NotSupportedException),
                typeof(InsufficientExecutionStackException),
                typeof(ToolException),
                typeof(EngineException),
            ],
        };

    [Fact]
    internal static Task IValuerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<IValuerChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task IValuerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<IValuerChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }
}
