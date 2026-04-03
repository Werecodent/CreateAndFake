using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.TesterTool.Guarders;
using CreateAndFake.Tests.TesterTool.TestSamples;

namespace CreateAndFake.Tests.TesterTool.Guarders;

public static class ExceptionGuarderTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IncludeInternals = false,
            IgnoreAllExceptions = true,
        };

    [Fact]
    internal static Task ExceptionGuarder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ExceptionGuarder>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task CallAllMethodsAsync_SuccessfulNoException()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync<InjectMockSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task CallAllMethodsAsync_FailsWithException()
    {
        return Tools
            .Tester.Assert(t =>
                t.PassthroughWithNoExceptionsAsync<MethodThrowsSample>(
                    TestContext.Current.CancellationToken
                )
            )
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task HandleCheckException_UsesAsserterFail([Fake] IAsserter asserter)
    {
        asserter
            .ToFake()
            .Setup(d => d.Fail(Arg.Any<Exception>(), Arg.Any<string>()), Behavior.None(Times.Once));

        await new ExceptionGuarder(
            Tools.Tester.Options with
            {
                Asserter = asserter,
            }
        ).CallAllMethodsAsync(new MethodThrowsSample(), TestContext.Current.CancellationToken);

        asserter.Assert().Called();
    }
}
