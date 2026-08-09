using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractorChainerTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(UnsupportedException),
                typeof(ToolException),
                typeof(TargetParameterCountException),
            ],
        };

    [Fact]
    internal static Task ExtractorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new ExtractorChainer(Tools.Extractor.Options, new ExtractorEngine()),
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    /*[Fact]
    internal static Task ExtractorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new ExtractorChainer(Tools.Extractor.Options, new ExtractorEngine()),
            TestContext.Current.CancellationToken,
            config
        );
    }*/
}
