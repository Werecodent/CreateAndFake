using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncObjectTests
{
    [Fact]
    internal static Task AssertAsyncObject_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ToolException)],
                }
        );
    }

    [Fact]
    internal static Task AssertAsyncObject_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ToolException)],
                }
        );
    }

    [Theory, RandomData]
    internal static async Task AssertAsyncObject_CallsAndChains(
        Injected<AssertAsyncObject> instance
    )
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertAsyncObject>)
            .Where(r => r.Result as string != nameof(AssertAsyncObject))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal static async Task IsAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await data.Assert().IsAsync(clone, canceler);
        await data.Assert().IsAsync(clone, canceler, mod);
        await data.Assert()
            .IsAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .IsAsync(variant, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task IsNotAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await data.Assert().IsNotAsync(variant, canceler);
        await data.Assert().IsNotAsync(variant, canceler, mod);
        await data.Assert()
            .IsNotAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .IsNotAsync(clone, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ValuesEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await data.Assert().ValuesEqualAsync(clone, canceler);
        await data.Assert().ValuesEqualAsync(clone, canceler, mod);
        await data.Assert()
            .ValuesEqualAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ValuesEqualAsync(variant, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ValuesNotEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await data.Assert().ValuesNotEqualAsync(variant, canceler);
        await data.Assert().ValuesNotEqualAsync(variant, canceler, mod);
        await data.Assert()
            .ValuesNotEqualAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ValuesNotEqualAsync(clone, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task UniqueFromAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        [Unique] AsyncDataSample unique
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await data.Assert().UniqueFromAsync(unique, canceler);
        await data.Assert().UniqueFromAsync(unique, canceler, mod);
        await data.Assert()
            .UniqueFromAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .UniqueFromAsync(clone, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }
}
