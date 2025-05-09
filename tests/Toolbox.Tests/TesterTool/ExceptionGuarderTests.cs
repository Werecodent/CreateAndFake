using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.Tests.TesterTool.TestSamples;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.TesterTool;

public static class ExceptionGuarderTests
{
    [Fact]
    internal static Task ExceptionGuarder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ExceptionGuarder>();
    }

    [Fact]
    internal static Task ExceptionGuarder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ExceptionGuarder>();
    }

    [Fact]
    internal static Task CallAllMethods_SuccessfulNoException()
    {
        return Tools.Tester.PassthroughWithNoExceptions<InjectMockSample>();
    }

    [Fact]
    internal static Task CallAllMethods_FailsWithException()
    {
        return Tools
            .Tester.Assert(t => t.PassthroughWithNoExceptions<MethodThrowsSample>())
            .Throws<AssertException>();
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
        ).CallAllMethods(new MethodThrowsSample());

        asserter.Assert().Called();
    }
}
