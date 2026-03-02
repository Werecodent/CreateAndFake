using System.Reflection;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

public static class DuplicatorChainerTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(UnsupportedException),
                typeof(TargetParameterCountException),
                typeof(ArgumentException),
                typeof(ToolException),
            ],
        };

    [Fact]
    internal static Task DuplicatorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<IDuplicatorChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task DuplicatorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<IDuplicatorChainer>(
            TestContext.Current.CancellationToken,
            config
        );
    }
}
