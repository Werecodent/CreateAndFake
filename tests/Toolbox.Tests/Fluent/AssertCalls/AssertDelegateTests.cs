using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertDelegateTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertDelegateTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

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
        error.Assert(x => false ? "" : throw x).Throws<Exception>().That().Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_CatchesExpected(ArgumentNullException error)
    {
        error.Assert(x => false ? "" : throw x).Throws<ArgumentNullException>().That().Is(error);
    }

    [Theory, RandomData]
    internal static void Throws_UnwrapsAggregate(InvalidOperationException error)
    {
        error
            .Assert(x => false ? "" : throw new AggregateException(x))
            .Throws<InvalidOperationException>()
            .That()
            .Is(error);
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
        string[] allowedResults =
        [
            "AssertChainer<AssertDelegate>",
            "ExceptionChainer<",
            nameof(AlsoChainer),
            "ResultChainer<",
            nameof(VoidType),
        ];

        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r =>
                !allowedResults.Any(x =>
                    GenericConverter
                        .ExpandName(r.Result?.GetType())
                        .Contains(x, StringComparison.Ordinal)
                )
            )
            .Where(r => r.Result as string != nameof(AssertDelegate))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void HasResult_ForwardedPlain(string data)
    {
        Delegate behavior = () => data;

        behavior.Assert().HasResult<string>();
        behavior.Assert().HasResult<string>(_mod);

        behavior.Assert(x => x.Assert().HasResult<int>()).Throws<AssertException>();
        behavior.Assert(x => x.Assert().HasResult<int>(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void HasResult_Forwarded(string valid, string invalid)
    {
        Delegate behavior = () => valid;

        behavior.Assert().HasResult(valid);
        behavior.Assert().HasResult(valid, _mod);

        behavior.Assert(x => x.Assert().HasResult(invalid)).Throws<AssertException>();
        behavior.Assert(x => x.Assert().HasResult(invalid, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task HasResultAsync_Forwarded(string valid, string invalid)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Delegate behavior = () => valid;

        await behavior.Assert().HasResultAsync(valid, canceler);
        await behavior.Assert().HasResultAsync(valid, canceler, _mod);

        await behavior
            .Assert()
            .HasResultAsync(invalid, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await behavior
            .Assert()
            .HasResultAsync(invalid, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void Throws_Forwarded(Delegate behavior, Exception error)
    {
        Delegate thrower = (Action)(() => throw error);

        thrower.Assert().Throws<Exception>();
        thrower.Assert().Throws<Exception>(_mod);

        behavior.Assert(x => x.Assert().Throws<Exception>()).Throws<AssertException>();
        behavior.Assert(x => x.Assert().Throws<Exception>(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ThrowsException_Forwarded(Func<string> behavior, Exception error)
    {
        Func<string> thrower = () => throw error;

        thrower.Assert().ThrowsException();
        thrower.Assert().ThrowsException(_mod);

        behavior.Assert(x => x.Assert().ThrowsException()).Throws<AssertException>();
        behavior.Assert(x => x.Assert().ThrowsException(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ThrowsNo_Forwarded(Delegate behavior, Exception error)
    {
        Delegate thrower = (Action)(() => throw error);

        behavior.Assert().ThrowsNo<Exception>();
        behavior.Assert().ThrowsNo<Exception>(_mod);
        thrower.Assert(x => x.Assert().ThrowsNo<Exception>()).Throws<AssertException>();
        thrower.Assert(x => x.Assert().ThrowsNo<Exception>(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ThrowsNoException_Forwarded(Func<int> behavior, Exception error)
    {
        Func<int> thrower = () => throw error;

        behavior.Assert().ThrowsNoException();
        behavior.Assert().ThrowsNoException(_mod);
        thrower.Assert(x => x.Assert().ThrowsNoException()).Throws<AssertException>();
        thrower.Assert(x => x.Assert().ThrowsNoException(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }
}
