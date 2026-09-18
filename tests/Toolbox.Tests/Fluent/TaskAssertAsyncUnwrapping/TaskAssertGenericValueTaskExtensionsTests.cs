using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public sealed class TaskAssertGenericValueTaskExtensionsTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertGenericValueTaskExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task TaskAssertGenericValueTaskExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertGenericValueTaskExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(AssertException),
                        typeof(ValueTaskRepeatedAccessException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task TaskAssertGenericValueTaskExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertGenericValueTaskExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(AssertException),
                        typeof(ValueTaskRepeatedAccessException),
                    ],
                }
        );
    }

    [Fact]
    internal static void TaskAssertGenericValueTaskExtensions_MatchesEveryMethod()
    {
        typeof(AssertGenericValueTaskBase<,>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Where(m => m != nameof(AssertGenericTaskBase<,>.ThrowsAsync))
            .Where(m => m != nameof(AssertGenericTaskBase<,>.ThrowsNoAsync))
            .Assert()
            .Is(
                typeof(TaskAssertGenericValueTaskExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
                    .Where(m =>
                        m != nameof(TaskAssertGenericValueTaskExtensions.ThrowsExceptionAsync)
                    )
                    .Where(m =>
                        m != nameof(TaskAssertGenericValueTaskExtensions.ThrowsNoExceptionAsync)
                    )
            );
    }

    [Theory, RandomData]
    internal async Task HasResultAsync_WithMatchForwarded(string value, string variant)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async ValueTask<string> getValue()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            return value;
        }

        await Task.FromResult(getValue().Assert()).HasResultAsync(value, canceler);
        await Task.FromResult(getValue().Assert()).HasResultAsync(value, canceler, _mod);
        await Task.FromResult(getValue().Assert())
            .HasResultAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(getValue().Assert())
            .HasResultAsync(variant, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task HasResultAsync_Forwarded(
        ValueTask<string> dataA,
        ValueTask<int> dataB,
        Exception error
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async ValueTask<DataSample> thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await Task.FromResult(dataA.Assert()).HasResultAsync(canceler);
        await Task.FromResult(dataB.Assert()).HasResultAsync(canceler, _mod);
        await Task.FromResult(thrower().Assert())
            .HasResultAsync(canceler)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);
        await Task.FromResult(thrower().Assert())
            .HasResultAsync(canceler, _mod)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsExceptionAsync_Forwarded(
        ValueTask<string> dataA,
        ValueTask<int> dataB,
        Exception error
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async ValueTask<DataSample> thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await Task.FromResult(thrower().Assert()).ThrowsExceptionAsync(canceler);
        await Task.FromResult(thrower().Assert()).ThrowsExceptionAsync(canceler, _mod);
        await Task.FromResult(dataA.Assert())
            .ThrowsExceptionAsync(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(dataB.Assert())
            .ThrowsExceptionAsync(canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ThrowsNoExceptionAsync_Forwarded(
        ValueTask<string> dataA,
        ValueTask<int> dataB,
        Exception error
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async ValueTask<DataSample> thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await Task.FromResult(dataA.Assert()).ThrowsNoExceptionAsync(canceler);
        await Task.FromResult(dataB.Assert()).ThrowsNoExceptionAsync(canceler, _mod);
        await Task.FromResult(thrower().Assert())
            .ThrowsNoExceptionAsync(canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(thrower().Assert())
            .ThrowsNoExceptionAsync(canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }
}
