using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertGenericValueTaskTests
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

    [Fact]
    internal static Task AssertGenericValueTask_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertGenericValueTask<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertGenericValueTask_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertGenericValueTask<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    /*[Theory, RandomData]
    internal static async Task AssertGenericValueTask_CallsAndChains(
        Injected<AssertGenericValueTask<DataSample>> instance
    )
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertGenericValueTask<DataSample>>)
            .Where(r => r.Result is not AlsoChainer)
            .Where(r => r.Result.GetType().Inherits(typeof(ExceptionChainer<>)))
            .Where(r => r.Result as string != "AssertGenericValueTask<DataSample>")
            .Assert()
            .IsEmpty();
    }*/

    [Theory, RandomData]
    internal static async Task HasResultAsync_WithMatchForwarded(string value, string variant)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        async ValueTask<string> getValue()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            return value;
        }

        await getValue().Assert().HasResultAsync(value, canceler);
        await getValue().Assert().HasResultAsync(value, canceler, mod);
        await getValue()
            .Assert()
            .HasResultAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await getValue()
            .Assert()
            .HasResultAsync(variant, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task HasResultAsync_Forwarded(
        ValueTask<string> dataA,
        ValueTask<int> dataB,
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
        async ValueTask<DataSample> thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await dataA.Assert().HasResultAsync(canceler);
        await dataB.Assert().HasResultAsync(canceler, mod);
        await thrower()
            .Assert()
            .HasResultAsync(canceler)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);
        await thrower()
            .Assert()
            .HasResultAsync(canceler, mod)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ThrowsAsync_Forwarded(
        ValueTask<string> dataA,
        ValueTask<int> dataB,
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
        async ValueTask<DataSample> thrower()
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
        ValueTask<string> dataA,
        ValueTask<int> dataB,
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
        async ValueTask<DataSample> thrower()
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
