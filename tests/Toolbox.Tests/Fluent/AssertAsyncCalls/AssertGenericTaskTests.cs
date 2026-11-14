using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Extensions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public sealed class AssertGenericTaskTests : AutoTester<AssertGenericTask<string>>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
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

    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertGenericTaskTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Theory, RandomData]
    internal async Task AssertGenericTask_CallsAndChains(
        Injected<AssertGenericTask<DataSample>> instance
    )
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertGenericTask<DataSample>>)
            .Where(r => r.Result is not AlsoChainer)
            .Where(r => r.Result is not ResultChainer<DataSample>)
            .Where(r => r.Result.GetType().Inherits(typeof(ExceptionChainer<>)))
            .Where(r => r.Result as string != "AssertGenericTask<DataSample>")
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal async Task HasResultAsync_WithMatchForwarded(string value, string variant)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async Task<string> getValue()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            return value;
        }

        await getValue().Assert().HasResultAsync(value, canceler);
        await getValue().Assert().HasResultAsync(value, canceler, _mod);
        await getValue()
            .Assert()
            .HasResultAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await getValue()
            .Assert()
            .HasResultAsync(variant, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task HasResultAsync_Forwarded(
        Task<string> dataA,
        Task<int> dataB,
        Exception error
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async Task<DataSample> thrower()
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            throw error;
        }

        await dataA.Assert().HasResultAsync(canceler);
        await dataB.Assert().HasResultAsync(canceler, _mod);
        await thrower()
            .Assert()
            .HasResultAsync(canceler)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);
        await thrower()
            .Assert()
            .HasResultAsync(canceler, _mod)
            .Assert()
            .ThrowsAsync<Exception>(canceler)
            .That()
            .Is(error);

        _modCount.Assert().Is(2);
    }
}
