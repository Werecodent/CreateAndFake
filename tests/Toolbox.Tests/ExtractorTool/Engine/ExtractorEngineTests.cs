using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool.Engine;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractorEngineTests
{
    private static readonly ExtractorEngine _TestInstance = new();

    [Fact]
    internal static Task ExtractorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ExtractorEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
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

    [Fact]
    internal static Task ExtractorEngine_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ExtractorEngine>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [Tools.Extractor.Options],
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

    [Theory, RandomData]
    internal static void Extract_NoHintsUnsupported(object data)
    {
        _TestInstance
            .Assert(x => x.Extract(data, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Extract_NullResultSafe([Stub] IExtractHint hint, object data)
    {
        _TestInstance
            .Assert(x => x.Extract(data, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static Task ExtractAsync_NoHintsUnsupported(object data)
    {
        return _TestInstance
            .ExtractAsync(data, CreateHintChainer(null), TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<UnsupportedException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task ExtractAsync_NullResultSafe([Stub] IExtractHint hint, object data)
    {
        hint.TryToExtractAsync(
                Arg.Any<object>(),
                Arg.Any<IExtractorChainer>(),
                Arg.Any<CancellationToken>()
            )
            .SetupReturn(Task.FromResult<ExtractHintResult>(null));

        return _TestInstance
            .ExtractAsync(data, CreateHintChainer(hint), TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<UnsupportedException>(TestContext.Current.CancellationToken);
    }

    private static ExtractorChainer CreateHintChainer(IExtractHint hint)
    {
        return new ExtractorChainer(
            Tools.Extractor.Options with
            {
                IncludeFoundHints = false,
                IncludeFrameworkHints = false,
                Hints = hint != null ? [hint] : [],
            },
            new ExtractorEngine()
        );
    }
}
