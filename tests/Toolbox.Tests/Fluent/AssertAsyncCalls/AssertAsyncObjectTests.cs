using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public sealed class AssertAsyncObjectTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertAsyncObjectTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

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
    internal async Task AssertAsyncObject_CallsAndChains(Injected<AssertAsyncObject> instance)
    {
        string[] allowedResults =
        [
            "AssertChainer<AssertAsyncObject",
            "ExceptionChainer<",
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
            .Where(r => r.Result as string != nameof(AssertAsyncObject))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal async Task IsAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await data.Assert().IsAsync(clone, canceler);
        await data.Assert().IsAsync(clone, canceler, _mod);
        await data.Assert()
            .IsAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .IsAsync(variant, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task IsNotAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await data.Assert().IsNotAsync(variant, canceler);
        await data.Assert().IsNotAsync(variant, canceler, _mod);
        await data.Assert()
            .IsNotAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .IsNotAsync(clone, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ValuesEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await data.Assert().ValuesEqualAsync(clone, canceler);
        await data.Assert().ValuesEqualAsync(clone, canceler, _mod);
        await data.Assert()
            .ValuesEqualAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ValuesEqualAsync(variant, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ValuesNotEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await data.Assert().ValuesNotEqualAsync(variant, canceler);
        await data.Assert().ValuesNotEqualAsync(variant, canceler, _mod);
        await data.Assert()
            .ValuesNotEqualAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ValuesNotEqualAsync(clone, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task UniqueFromAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        [Unique] AsyncDataSample unique
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await data.Assert().UniqueFromAsync(unique, canceler);
        await data.Assert().UniqueFromAsync(unique, canceler, _mod);
        await data.Assert()
            .UniqueFromAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .UniqueFromAsync(clone, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }
}
