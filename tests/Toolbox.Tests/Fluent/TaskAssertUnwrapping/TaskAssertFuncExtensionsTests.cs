using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public sealed class TaskAssertFuncExtensionsTests : AutoTester
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(AssertException), typeof(ArgumentException)],
            };

    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertFuncExtensionsTests()
        : base(typeof(TaskAssertFuncExtensions))
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static void TaskAssertFuncExtensions_MatchesEveryMethod()
    {
        typeof(AssertFuncBase<,>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertFuncExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }

    [Theory, RandomData]
    internal async Task HasResult_Forwarded(Func<string> behavior, Exception error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        Func<string> thrower = () => throw error;

        await Task.FromResult(behavior.Assert()).HasResult();
        await Task.FromResult(behavior.Assert()).HasResult(_mod);

        await Task.FromResult(thrower.Assert())
            .HasResult()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(thrower.Assert())
            .HasResult(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }
}
