using System.Collections.Frozen;
using CreateAndFake.FakerTool.Engine;

namespace CreateAndFake.Tests.FakerTool.Engine;

public static class FakerEngineTests
{
    [Fact]
    internal static Task FakerEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new FakerEngine(),
            TestContext.Current.CancellationToken,
            opt => opt with { MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints", "Inject"]) }
        );
    }

    [Fact]
    internal static Task FakerEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new FakerEngine(),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints", "Inject"]),
                    IgnorableExceptions = [typeof(ArgumentException)],
                }
        );
    }
}
