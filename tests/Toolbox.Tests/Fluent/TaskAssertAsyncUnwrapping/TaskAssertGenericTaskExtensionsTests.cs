using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public sealed class TaskAssertGenericTaskExtensionsTests : AutoTester
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(AssertException)] };

    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertGenericTaskExtensionsTests()
        : base(typeof(TaskAssertGenericTaskExtensions))
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static void TaskAssertGenericTaskExtensions_MatchesEveryMethod()
    {
        typeof(AssertGenericTaskBase<,>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertGenericTaskExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
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
}
