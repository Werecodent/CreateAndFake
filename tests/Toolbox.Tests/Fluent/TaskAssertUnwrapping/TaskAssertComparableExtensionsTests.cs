using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public sealed class TaskAssertComparableExtensionsTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertComparableExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task TaskAssertComparableExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertComparableExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static Task TaskAssertComparableExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertComparableExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
                }
        );
    }

    [Fact]
    internal static void TaskAssertComparableExtensions_MatchesEveryMethod()
    {
        typeof(AssertComparableBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertComparableExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }

    [Theory, RandomData]
    internal async Task GreaterThan_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).GreaterThan(less);
        await Task.FromResult(data.Assert()).GreaterThan(less, _mod);
        await Task.FromResult(data.Assert())
            .GreaterThan(same)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .GreaterThan(more, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task GreaterThanOrEqualTo_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).GreaterThanOrEqualTo(less);
        await Task.FromResult(data.Assert()).GreaterThanOrEqualTo(same, _mod);
        await Task.FromResult(data.Assert())
            .GreaterThanOrEqualTo(more)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .GreaterThanOrEqualTo(more, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task GreaterThanOrIs_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).GreaterThanOrIs(less);
        await Task.FromResult(data.Assert()).GreaterThanOrIs(same, _mod);
        await Task.FromResult(data.Assert())
            .GreaterThanOrIs(more)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .GreaterThanOrIs(more, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task LessThan_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).LessThan(more);
        await Task.FromResult(data.Assert()).LessThan(more, _mod);
        await Task.FromResult(data.Assert())
            .LessThan(same)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .LessThan(less, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task LessThanOrEqualTo_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).LessThanOrEqualTo(more);
        await Task.FromResult(data.Assert()).LessThanOrEqualTo(same, _mod);
        await Task.FromResult(data.Assert())
            .LessThanOrEqualTo(less)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .LessThanOrEqualTo(less, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task LessThanOrIs_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).LessThanOrIs(more);
        await Task.FromResult(data.Assert()).LessThanOrIs(same, _mod);
        await Task.FromResult(data.Assert())
            .LessThanOrIs(less)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .LessThanOrIs(less, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task InRange_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).InRange(less, same);
        await Task.FromResult(data.Assert()).InRange(same, more, _mod);
        await Task.FromResult(data.Assert())
            .InRange(less, less)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .InRange(more, more, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }
}
