using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Extensions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public sealed class AssertTaskTests
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
                typeof(ValueTaskRepeatedAccessException),
            ],
        };

    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertTaskTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task AssertTask_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertTask>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertTask_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertTask>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal async Task AssertTask_CallsAndChains(Injected<AssertTask> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertTask>)
            .Where(r => r.Result is not AlsoChainer)
            .Where(r => r.Result.GetType().Inherits(typeof(ExceptionChainer<>)))
            .Where(r => r.Result as string != nameof(AssertTask))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal async Task ThrowsAsync_Forwarded(Task data, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async Task thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await thrower().Assert().ThrowsAsync<Exception>(canceler);
        await thrower().Assert().ThrowsAsync<Exception>(canceler, _mod);
        await data.Assert()
            .ThrowsAsync<Exception>(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ThrowsAsync<Exception>(canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsExceptionAsync_Forwarded(Task data, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async Task thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await thrower().Assert().ThrowsExceptionAsync(canceler);
        await thrower().Assert().ThrowsExceptionAsync(canceler, _mod);
        await data.Assert()
            .ThrowsExceptionAsync(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ThrowsExceptionAsync(canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsNoAsync_Forwarded(Task data, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async Task thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await data.Assert().ThrowsNoAsync<Exception>(canceler);
        await data.Assert().ThrowsNoAsync<Exception>(canceler, _mod);
        await thrower()
            .Assert()
            .ThrowsNoAsync<Exception>(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await thrower()
            .Assert()
            .ThrowsNoAsync<Exception>(canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsNoExceptionAsync_Forwarded(Task data, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async Task thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await data.Assert().ThrowsNoExceptionAsync(canceler);
        await data.Assert().ThrowsNoExceptionAsync(canceler, _mod);
        await thrower()
            .Assert()
            .ThrowsNoExceptionAsync(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await thrower()
            .Assert()
            .ThrowsNoExceptionAsync(canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }
}
