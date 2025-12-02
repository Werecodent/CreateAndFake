using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.TesterTool;
using CreateAndFake.Tests.TesterTool.TestSamples;

namespace CreateAndFake.Tests.TesterTool;

public static class ExceptionGuarderTests
{
    private static readonly TesterMod config = opt => opt with { IgnoreAllExceptions = true };

    [Fact]
    internal static Task ExceptionGuarder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ExceptionGuarder>(config);
    }

    [Fact]
    internal static Task ExceptionGuarder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ExceptionGuarder>(config);
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
