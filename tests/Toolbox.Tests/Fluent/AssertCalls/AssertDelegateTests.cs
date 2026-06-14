using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertDelegateTests
{
    [Fact]
    internal static Task AssertDelegate_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertDelegate>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }

    [Fact]
    internal static Task AssertDelegate_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertDelegate>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }

    [Theory, RandomData]
    internal static void Throws_ReturnsException(Exception error)
    {
        error.Assert(e => false ? "" : throw e).Throws<Exception>().That.Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_CatchesExpected(ArgumentNullException error)
    {
        error.Assert(e => false ? "" : throw e).Throws<ArgumentNullException>().That.Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_UnwrapsAggregate(InvalidOperationException error)
    {
        error
            .Assert(e => false ? "" : throw new AggregateException(e))
            .Throws<InvalidOperationException>()
            .That.Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_ActionNoException(Action behavior)
    {
        behavior.Assert(d => d.Assert().Throws<Exception>()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Throws_FuncNoException(Func<object> behavior)
    {
        behavior.Assert(d => d.Assert().Throws<Exception>()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static Task Throws_WrongException(ArgumentNullException error)
    {
        return error
            .Assert(e =>
                e.Assert(ex => throw ex)
                    .ThrowsAsync<InvalidOperationException>(TestContext.Current.CancellationToken)
            )
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static void Throws_OptionsOkay(ArgumentNullException error)
    {
        error.Assert(e =>
            e.Assert(ex => throw ex)
                .ThrowsAsync<ArgumentNullException>(
                    TestContext.Current.CancellationToken,
                    opt => opt
                )
        );
    }

    [Theory, RandomData]
    internal static Task Throws_WrongAggregate(InvalidOperationException error)
    {
        return error
            .Assert(e =>
                e.Assert(ex => throw new AggregateException(ex))
                    .ThrowsAsync<ArgumentNullException>(TestContext.Current.CancellationToken)
            )
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task Throws_TooManyAggregate(
        ArgumentNullException error,
        InvalidOperationException error2
    )
    {
        return error
            .Assert(e =>
                error2
                    .Assert(ex => throw new AggregateException(e, ex))
                    .ThrowsAsync<ArgumentNullException>(TestContext.Current.CancellationToken)
            )
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static void ThrowsNo_NoopAction(Action behavior)
    {
        behavior.Assert().ThrowsNo<Exception>();
    }

    [Theory, RandomData]
    internal static void ThrowsNo_NoopFunc(Func<object> behavior)
    {
        behavior.Assert().ThrowsNo<Exception>();
    }

    [Theory, RandomData]
    internal static Task ThrowsNo_Error(Exception error)
    {
        return error
            .Assert(e =>
                e.Assert(ex => throw ex)
                    .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken)
            )
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task ThrowsNo_DifferentExceptionIgnored(TimeoutException error)
    {
        return error
            .Assert(e =>
                e.Assert(ex => throw ex)
                    .ThrowsNoAsync<IOException>(TestContext.Current.CancellationToken)
            )
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task AssertDelegate_CallsAndChains(Injected<AssertDelegate> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result is not AssertChainer<AssertDelegate>
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ResultChainer<>))
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ExceptionChainer<>))
                && r.Result is not AlsoChainer
            )
            .Select(r => r.Method.Name)
            .Assert()
            .IsEmpty();
    }
}
