using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertEnumerableTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ToolException),
                typeof(InvalidCastException),
                typeof(UnsupportedException),
                typeof(ArgumentException),
                typeof(TargetException),
            ],
        };

    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertEnumerableTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task AssertEnumerable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertEnumerable>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertEnumerable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertEnumerable>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertEnumerable_CallsAndChains(Injected<AssertEnumerable> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r =>
                r.Result
                    is not AssertChainer<AssertEnumerable>
                        and Task<AssertChainer<AssertEnumerable>>
            )
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void IsEmpty_Forwarded(
        [Size(0)] IEnumerable<int> valid,
        [Size(1)] IEnumerable<int> invalid
    )
    {
        valid.Assert().IsEmpty();
        valid.Assert().IsEmpty(_mod);
        invalid.Assert(x => x.Assert().IsEmpty()).Throws<AssertException>();
        invalid.Assert(x => x.Assert().IsEmpty(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void IsNotEmpty_Forwarded(
        [Size(1)] IEnumerable<int> valid,
        [Size(0)] IEnumerable<int> invalid
    )
    {
        valid.Assert().IsNotEmpty();
        valid.Assert().IsNotEmpty(_mod);
        invalid.Assert(x => x.Assert().IsNotEmpty()).Throws<AssertException>();
        invalid.Assert(x => x.Assert().IsNotEmpty(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void HasCount_Forwarded([Size(1)] IEnumerable<int> data)
    {
        data.Assert().HasCount(1);
        data.Assert().HasCount(1, _mod);
        data.Assert(x => x.Assert().HasCount(0)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCount(2, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void HasCountLessThan_Forwarded([Size(1)] IEnumerable<int> data)
    {
        data.Assert().HasCountLessThan(2);
        data.Assert().HasCountLessThan(2, _mod);
        data.Assert(x => x.Assert().HasCountLessThan(1)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountLessThan(0, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void HasCountLessOrExactly_Forwarded([Size(1)] IEnumerable<int> data)
    {
        data.Assert().HasCountLessOrExactly(2);
        data.Assert().HasCountLessOrExactly(1, _mod);
        data.Assert(x => x.Assert().HasCountLessOrExactly(0)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountLessOrExactly(0, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void HasCountMoreThan_Forwarded([Size(1)] IEnumerable<int> data)
    {
        data.Assert().HasCountMoreThan(0);
        data.Assert().HasCountMoreThan(0, _mod);
        data.Assert(x => x.Assert().HasCountMoreThan(1)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountMoreThan(2, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void HasCountMoreOrExactly_Forwarded([Size(1)] IEnumerable<int> data)
    {
        data.Assert().HasCountMoreOrExactly(0);
        data.Assert().HasCountMoreOrExactly(1, _mod);
        data.Assert(x => x.Assert().HasCountMoreOrExactly(2)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountMoreOrExactly(2, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void Contains_Forwarded(int valid, int invalid)
    {
        IEnumerable<int> data = [valid];

        data.Assert().Contains(valid);
        data.Assert().Contains(valid, _mod);
        data.Assert(x => x.Assert().Contains(invalid)).Throws<AssertException>();
        data.Assert(x => x.Assert().Contains(invalid, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ContainsAsync_Forwarded(AsyncDataSample valid, AsyncDataSample invalid)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        IEnumerable<AsyncDataSample> data = [valid];

        await data.Assert().ContainsAsync(valid, canceler);
        await data.Assert().ContainsAsync(valid, canceler, _mod);
        await data.Assert()
            .ContainsAsync(invalid, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ContainsAsync(invalid, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ContainsNot_Forwarded(int valid, int invalid)
    {
        IEnumerable<int> data = [invalid];

        data.Assert().ContainsNot(valid);
        data.Assert().ContainsNot(valid, _mod);
        data.Assert(x => x.Assert().ContainsNot(invalid)).Throws<AssertException>();
        data.Assert(x => x.Assert().ContainsNot(invalid, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ContainsNotAsync_Forwarded(AsyncDataSample valid, AsyncDataSample invalid)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        IEnumerable<AsyncDataSample> data = [invalid];

        await data.Assert().ContainsNotAsync(valid, canceler);
        await data.Assert().ContainsNotAsync(valid, canceler, _mod);
        await data.Assert()
            .ContainsNotAsync(invalid, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ContainsNotAsync(invalid, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void Fail_Forwarded(IEnumerable<int> data, [Fake] IAsserter asserter)
    {
        ToolSet silentFailSet = MakeSet(asserter);

        data.Assert(silentFailSet).Fail();
        data.Assert(silentFailSet).Fail(_mod);
        data.Assert(x => x.Assert().Fail()).Throws<AssertException>();
        data.Assert(x => x.Assert().Fail(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal void Debug_Forwarded(IEnumerable<int> data)
    {
        ToolSet debugPassSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = false })
        );
        ToolSet debugFailSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = true })
        );

        data.Assert(debugPassSet).Debug();
        data.Assert(debugPassSet).Debug(_mod);
        data.Assert(x => x.Assert(debugFailSet).Debug()).Throws<AssertException>();
        data.Assert(x => x.Assert(debugFailSet).Debug(_mod)).Throws<AssertException>();

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
