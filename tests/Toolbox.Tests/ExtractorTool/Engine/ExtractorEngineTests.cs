using System.Collections.Frozen;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractorEngineTests
{
    [Fact]
    internal static Task ExtractorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ExtractorEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                    IgnorableExceptions =
                    [
                        typeof(ToolException),
                        typeof(UnsupportedException),
                        typeof(TargetParameterCountException),
                        typeof(InvalidOperationException),
                    ],
                }
        );
    }

    /*[Fact]
    internal static Task ExtractorEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ExtractorEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Extractor.Options],
                    MethodsToIgnore = FrozenSet.ToFrozenSet(["SelectHints"]),
                    IgnorableExceptions =
                    [
                        typeof(ToolException),
                        typeof(UnsupportedException),
                        typeof(TargetParameterCountException),
                        typeof(InvalidOperationException),
                    ],
                }
        );
    }*/
}
