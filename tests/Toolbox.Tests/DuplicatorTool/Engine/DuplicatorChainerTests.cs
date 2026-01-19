using System.Reflection;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class DuplicatorChainerTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(NotSupportedException),
                typeof(TargetParameterCountException),
                typeof(ArgumentException),
                typeof(ToolException),
            ],
        };

    [Fact]
    internal static Task DuplicatorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<IDuplicatorChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task DuplicatorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<IDuplicatorChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }
}
