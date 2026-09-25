using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public sealed class TaskAssertDelegateExtensionsTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertDelegateExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task TaskAssertDelegateExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertDelegateExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static Task TaskAssertDelegateExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertDelegateExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static void TaskAssertDelegateExtensions_MatchesEveryMethod()
    {
        typeof(AssertDelegateBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertDelegateExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }

    [Theory, RandomData]
    internal async Task HasResult_ForwardedPlain(string data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Delegate behavior = () => data;

        await Task.FromResult(behavior.Assert()).HasResult<string>();
        await Task.FromResult(behavior.Assert()).HasResult<string>(_mod);

        await Task.FromResult(behavior.Assert())
            .HasResult<int>()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(behavior.Assert())
            .HasResult<int>(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task HasResult_Forwarded(string valid, string invalid)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Delegate behavior = () => valid;

        await Task.FromResult(behavior.Assert()).HasResult(valid);
        await Task.FromResult(behavior.Assert()).HasResult(valid, _mod);

        await Task.FromResult(behavior.Assert())
            .HasResult(invalid)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(behavior.Assert())
            .HasResult(invalid, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task HasResultAsync_Forwarded(string valid, string invalid)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Delegate behavior = () => valid;

        await Task.FromResult(behavior.Assert()).HasResultAsync(valid, canceler);
        await Task.FromResult(behavior.Assert()).HasResultAsync(valid, canceler, _mod);

        await Task.FromResult(behavior.Assert())
            .HasResultAsync(invalid, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(behavior.Assert())
            .HasResultAsync(invalid, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task Throws_Forwarded(Delegate behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Delegate thrower = (Action)(() => throw error);

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
    internal async Task ThrowsException_Forwarded(Func<string> behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Func<string> thrower = () => throw error;

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
    internal async Task ThrowsNo_Forwarded(Delegate behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Delegate thrower = (Action)(() => throw error);

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
    internal async Task ThrowsNoException_Forwarded(Func<int> behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Func<int> thrower = () => throw error;

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
