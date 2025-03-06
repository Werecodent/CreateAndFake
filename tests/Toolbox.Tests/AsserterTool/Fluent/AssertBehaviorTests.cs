using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Fluent;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public static class AssertBehaviorTests
{
    [Fact]
    internal static void AssertBehavior_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertBehavior>();
    }

    [Fact]
    internal static void AssertBehavior_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertBehavior>();
    }

    [Theory, RandomData]
    internal static void Throws_ReturnsException(Exception error)
    {
        error.Assert(e => false ? "" : throw e).Throws<Exception>().Assert().Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_CatchesExpected(ArgumentNullException error)
    {
        error.Assert(e => false ? "" : throw e).Throws<ArgumentNullException>().Assert().Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_UnwrapsAggregate(InvalidOperationException error)
    {
        error
            .Assert(e => false ? "" : throw new AggregateException(e))
            .Throws<InvalidOperationException>()
            .Assert()
            .Is(error);
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
    internal static void Throws_WrongException(ArgumentNullException error)
    {
        error
            .Assert(e => e.Assert(ex => throw ex).Throws<InvalidOperationException>())
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void Throws_OptionsOkay(ArgumentNullException error)
    {
        error.Assert(e => e.Assert(ex => throw ex).Throws<ArgumentNullException>(opt => opt));
    }

    [Theory, RandomData]
    internal static void Throws_WrongAggregate(InvalidOperationException error)
    {
        error
            .Assert(e =>
                e.Assert(ex => throw new AggregateException(ex)).Throws<ArgumentNullException>()
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
            .Assert(e =>
                error2
                    .Assert(ex => throw new AggregateException(e, ex))
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
        error.Assert(e => e.Assert(ex => throw ex).ThrowsNo<Exception>()).Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void ThrowsNo_DifferentExceptionIgnored(TimeoutException error)
    {
        error
            .Assert(e => e.Assert(ex => throw ex).ThrowsNo<IOException>())
            .ThrowsNo<AssertException>();
    }
}
