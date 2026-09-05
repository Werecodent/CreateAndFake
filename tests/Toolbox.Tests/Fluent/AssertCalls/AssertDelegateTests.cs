using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertDelegateTests
{
    [Fact]
    internal static Task AssertDelegate_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertDelegate>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ToolException)],
                }
        );
    }

    [Fact]
    internal static Task AssertDelegate_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertDelegate>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ToolException)],
                }
        );
    }

    [Theory, RandomData]
    internal static void Throws_ReturnsException(Exception error)
    {
        error.Assert(x => false ? "" : throw x).Throws<Exception>().That.Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_CatchesExpected(ArgumentNullException error)
    {
        error.Assert(x => false ? "" : throw x).Throws<ArgumentNullException>().That.Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_UnwrapsAggregate(InvalidOperationException error)
    {
        error
            .Assert(x => false ? "" : throw new AggregateException(x))
            .Throws<InvalidOperationException>()
            .That.Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_ActionNoException(Action behavior)
    {
        behavior.Assert(x => x.Assert().Throws<Exception>()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Throws_FuncNoException(Func<object> behavior)
    {
        behavior.Assert(x => x.Assert().Throws<Exception>()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Throws_WrongException(ArgumentNullException error)
    {
        error
            .Assert(x => x.Assert(ex => throw ex).Throws<InvalidOperationException>())
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Throws_OptionsOkay(ArgumentNullException error)
    {
        error
            .Assert(x => x.Assert(ex => throw ex).Throws<ArgumentNullException>(opt => opt))
            .ThrowsNo<Exception>();
    }

    [Theory, RandomData]
    internal static void Throws_WrongAggregate(InvalidOperationException error)
    {
        error
            .Assert(x =>
                x.Assert(ex => throw new AggregateException(ex)).Throws<ArgumentNullException>()
            )
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Throws_TooManyAggregate(
        ArgumentNullException error,
        InvalidOperationException error2
    )
    {
        error
            .Assert(x =>
                error2
                    .Assert(ex => throw new AggregateException(x, ex))
                    .Throws<ArgumentNullException>()
            )
            .Throws<AssertException>();
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
    internal static void ThrowsNo_Error(Exception error)
    {
        error.Assert(x => x.Assert(ex => throw ex).ThrowsNo<Exception>()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ThrowsNo_DifferentExceptionIgnored(TimeoutException error)
    {
        error
            .Assert(x => x.Assert(ex => throw ex).ThrowsNo<IOException>())
            .ThrowsNo<AssertException>();
    }

    [Theory, RandomData]
    internal static async Task AssertDelegate_CallsAndChains(Injected<AssertDelegate> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result is not AssertChainer<AssertDelegate>
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ResultChainer<>))
                && !TypeDescriber.For(r.Result?.GetType()).Inherits(typeof(ExceptionChainer<>))
                && r.Result is not AlsoChainer
            )
            .Assert()
            .IsEmpty();
    }
}
