using System.Collections.Frozen;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Engine;

public static class DuplicatorEngineTests
{
    [Fact]
    internal static Task DuplicatorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DuplicatorEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(TargetParameterCountException),
                        typeof(UnsupportedException),
                        typeof(ToolException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task DuplicatorEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DuplicatorEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Duplicator.Options],
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                    IgnorableExceptions =
                    [
                        typeof(ArgumentException),
                        typeof(TargetParameterCountException),
                        typeof(UnsupportedException),
                        typeof(ToolException),
                    ],
                }
        );
    }
}
