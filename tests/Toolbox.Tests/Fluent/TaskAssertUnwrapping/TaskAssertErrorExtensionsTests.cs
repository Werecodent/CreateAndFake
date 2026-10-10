using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public sealed class TaskAssertErrorExtensionsTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertErrorExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task TaskAssertErrorExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertErrorExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskAssertErrorExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertErrorExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static void TaskAssertErrorExtensions_MatchesEveryMethod()
    {
        typeof(AssertErrorBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertErrorExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
                    .Where(m => m != nameof(AssertObjectBase<>.Inherits))
            );
    }

    [Theory, RandomData]
    internal async Task Inherits_Forwarded(AggregateException error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(error.Assert()).Inherits<Exception>();
        await Task.FromResult(error.Assert()).Inherits<Exception>(_mod);
        await Task.FromResult(error.Assert())
            .Inherits<InvalidOperationException>()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(error.Assert())
            .Inherits<InvalidOperationException>(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task Fail_Forwarded(Exception data, [Fake] IAsserter asserter)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        ToolSet silentFailSet = MakeSet(asserter);

        await Task.FromResult(data.Assert(silentFailSet)).Fail();
        await Task.FromResult(data.Assert(silentFailSet)).Fail(_mod);
        await Task.FromResult(data.Assert()).Fail().Assert().ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Fail(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal async Task Debug_Forwarded(Exception data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        ToolSet debugPassSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = false })
        );
        ToolSet debugFailSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = true })
        );

        await Task.FromResult(data.Assert(debugPassSet)).Debug();
        await Task.FromResult(data.Assert(debugPassSet)).Debug(_mod);
        await Task.FromResult(data.Assert(debugFailSet))
            .Debug()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert(debugFailSet))
            .Debug(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    private static ToolSet MakeSet(IAsserter asserter)
    {
        return new(
            Tools.Gen,
            Tools.Valuer,
            Tools.Faker,
            Tools.Randomizer,
            Tools.Extractor,
            Tools.Mutator,
            asserter,
            Tools.Duplicator,
            Tools.Runner,
            Tools.Tester
        );
    }
}
