using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public sealed class TaskAssertActionExtensionsTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertActionExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task TaskAssertActionExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertActionExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static Task TaskAssertActionExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertActionExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static void TaskAssertActionExtensions_MatchesEveryMethod()
    {
        typeof(AssertActionBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertActionExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }

    [Theory, RandomData]
    internal async Task Throws_Forwarded(Action behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Action thrower = () => throw error;

        await Task.FromResult(thrower.Assert()).Throws<Exception>();
        await Task.FromResult(thrower.Assert()).Throws<Exception>(_mod);

        await Task.FromResult(behavior.Assert())
            .Throws<Exception>()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(behavior.Assert())
            .Throws<Exception>(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsException_Forwarded(Action behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Action thrower = () => throw error;

        await Task.FromResult(thrower.Assert()).ThrowsException();
        await Task.FromResult(thrower.Assert()).ThrowsException(_mod);

        await Task.FromResult(behavior.Assert())
            .ThrowsException()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(behavior.Assert())
            .ThrowsException(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsNo_Forwarded(Action behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Action thrower = () => throw error;

        await Task.FromResult(behavior.Assert()).ThrowsNo<Exception>();
        await Task.FromResult(behavior.Assert()).ThrowsNo<Exception>(_mod);
        await Task.FromResult(thrower.Assert())
            .ThrowsNo<Exception>()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(thrower.Assert())
            .ThrowsNo<Exception>(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsNoException_Forwarded(Action behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Action thrower = () => throw error;

        await Task.FromResult(behavior.Assert()).ThrowsNoException();
        await Task.FromResult(behavior.Assert()).ThrowsNoException(_mod);
        await Task.FromResult(thrower.Assert())
            .ThrowsNoException()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(thrower.Assert())
            .ThrowsNoException(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }
}
