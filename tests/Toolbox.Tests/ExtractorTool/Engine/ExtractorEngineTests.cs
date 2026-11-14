using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.ExtractorTool.Engine;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Engine;

public sealed class ExtractorEngineTests : AutoTester<ExtractorEngine>
{
    protected override TesterMod OptionConfiguration { get; } =
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
            };

    [Theory, RandomData]
    internal void Extract_NoHintsUnsupported(object data)
    {
        TestInstance
            .Assert(x => x.Extract(data, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal void Extract_NullResultSafe([Stub] IExtractHint hint, object data)
    {
        TestInstance
            .Assert(x => x.Extract(data, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal Task ExtractAsync_NoHintsUnsupported(object data)
    {
        return TestInstance
            .ExtractAsync(data, CreateHintChainer(null), TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<UnsupportedException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task ExtractAsync_NullResultSafe([Stub] IExtractHint hint, object data)
    {
        hint.TryToExtractAsync(
                Arg.Any<object>(),
                Arg.Any<IExtractorChainer>(),
                Arg.Any<CancellationToken>()
            )
            .SetupReturn(Task.FromResult<ExtractHintResult>(null));

        return TestInstance
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
