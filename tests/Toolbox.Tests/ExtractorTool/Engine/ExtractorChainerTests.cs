using System.Reflection;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractorChainerTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(NotSupportedException),
                typeof(ToolException),
                typeof(TargetParameterCountException),
            ],
        };

    [Fact]
    internal static Task ExtractorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            new ExtractorChainer(
                Tools.Extractor.Options,
                new ExtractorEngine(Extractor.DefaultHints)
            ),
            config
        );
    }

    [Fact]
    internal static Task ExtractorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            new ExtractorChainer(
                Tools.Extractor.Options,
                new ExtractorEngine(Extractor.DefaultHints)
            ),
            config
        );
    }
}
