using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertValueTaskTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ToolException),
                typeof(InvalidCastException),
                typeof(ArgumentException),
            ],
        };

    [Fact]
    internal static Task AssertValueTask_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertValueTask>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertValueTask_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertValueTask>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertValueTask_CallsAndChains(Injected<AssertValueTask> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result is not Task<AssertChainer<AssertValueTask>>
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ExceptionChainer<>))
                && r.Result is not AlsoChainer
                && r.Result as string != nameof(AssertValueTask)
            )
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal static async Task ThrowsAsync_Forwarded(
        ValueTask dataA,
        ValueTask dataB,
        Exception error
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        async ValueTask thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await thrower().Assert().ThrowsAsync<Exception>(canceler);
        await thrower().Assert().ThrowsAsync<Exception>(canceler, mod);
        await dataA
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await dataB
            .Assert()
            .ThrowsAsync<Exception>(canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ThrowsNoAsync_Forwarded(
        ValueTask dataA,
        ValueTask dataB,
        Exception error
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        async ValueTask thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await dataA.Assert().ThrowsNoAsync<Exception>(canceler);
        await dataB.Assert().ThrowsNoAsync<Exception>(canceler, mod);
        await thrower()
            .Assert()
            .ThrowsNoAsync<Exception>(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await thrower()
            .Assert()
            .ThrowsNoAsync<Exception>(canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }
}
