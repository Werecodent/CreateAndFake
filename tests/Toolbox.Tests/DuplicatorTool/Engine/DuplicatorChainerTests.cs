using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Engine;

public static class DuplicatorChainerTests
{
    private static readonly TesterMod _Config = opt =>
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
            _Config
        );
    }

    [Fact]
    internal static Task DuplicatorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<IDuplicatorChainer>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }
}
