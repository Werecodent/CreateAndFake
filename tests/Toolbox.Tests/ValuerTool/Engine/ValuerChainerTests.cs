using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerChainerTests
{
    private static readonly TesterMod _Config = opt =>
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
            _Config
        );
    }

    [Fact]
    internal static Task ValuerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<IValuerChainer>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }
}
