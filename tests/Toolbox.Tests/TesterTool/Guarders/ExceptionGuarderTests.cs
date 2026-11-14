using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.TesterTool.Guarders;
using Werecodent.CreateAndFake.Tests.TesterTool.TestSamples;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Guarders;

public static class ExceptionGuarderTests
{
    [Fact]
    internal static Task CallAllMethodsAsync_SuccessfulNoException()
    {
        return Tools.Tester.PassthroughWithNoExceptionsAsync<InjectMockSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static async Task CallAllMethodsAsync_FailsWithException()
    {
        await Tools
            .Tester.PassthroughWithNoExceptionsAsync<MethodThrowsSample>(
                TestContext.Current.CancellationToken,
                opt => opt with { DisablePassthroughTests = false }
            )
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        await Tools
            .Tester.PassthroughWithNoExceptionsAsync(
                Tools.Randomizer.Create<MethodThrowsSample>(),
                TestContext.Current.CancellationToken,
                opt => opt with { DisablePassthroughTests = false }
            )
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task HandleCheckException_UsesAsserterFail([Fake] IAsserter asserter)
    {
        asserter
            .Tools()
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

    [Theory, RandomData]
    internal static async Task PassthroughWithNoExceptionsAsync_CanBeDisabled(
        MethodThrowsSample sample
    )
    {
        await Tools.Tester.PassthroughWithNoExceptionsAsync(
            sample,
            TestContext.Current.CancellationToken,
            opt => opt with { DisablePassthroughTests = true }
        );

        await Tools.Tester.PassthroughWithNoExceptionsAsync<MethodThrowsSample>(
            TestContext.Current.CancellationToken,
            opt => opt with { DisablePassthroughTests = true }
        );
    }
}
