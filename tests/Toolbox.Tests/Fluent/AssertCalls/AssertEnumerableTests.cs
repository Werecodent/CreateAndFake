using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertEnumerableTests
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
    internal static void IsEmpty_Forwarded(
        [Size(0)] IEnumerable<int> valid,
        [Size(1)] IEnumerable<int> invalid
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        valid.Assert().IsEmpty();
        valid.Assert().IsEmpty(mod);
        invalid.Assert(x => x.Assert().IsEmpty()).Throws<AssertException>();
        invalid.Assert(x => x.Assert().IsEmpty(mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void IsNotEmpty_Forwarded(
        [Size(1)] IEnumerable<int> valid,
        [Size(0)] IEnumerable<int> invalid
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        valid.Assert().IsNotEmpty();
        valid.Assert().IsNotEmpty(mod);
        invalid.Assert(x => x.Assert().IsNotEmpty()).Throws<AssertException>();
        invalid.Assert(x => x.Assert().IsNotEmpty(mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void HasCount_Forwarded([Size(1)] IEnumerable<int> data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().HasCount(1);
        data.Assert().HasCount(1, mod);
        data.Assert(x => x.Assert().HasCount(0)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCount(2, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void HasCountLessThan_Forwarded([Size(1)] IEnumerable<int> data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().HasCountLessThan(2);
        data.Assert().HasCountLessThan(2, mod);
        data.Assert(x => x.Assert().HasCountLessThan(1)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountLessThan(0, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void HasCountLessOrExactly_Forwarded([Size(1)] IEnumerable<int> data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().HasCountLessOrExactly(2);
        data.Assert().HasCountLessOrExactly(1, mod);
        data.Assert(x => x.Assert().HasCountLessOrExactly(0)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountLessOrExactly(0, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void HasCountMoreThan_Forwarded([Size(1)] IEnumerable<int> data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().HasCountMoreThan(0);
        data.Assert().HasCountMoreThan(0, mod);
        data.Assert(x => x.Assert().HasCountMoreThan(1)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountMoreThan(2, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void HasCountMoreOrExactly_Forwarded([Size(1)] IEnumerable<int> data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().HasCountMoreOrExactly(0);
        data.Assert().HasCountMoreOrExactly(1, mod);
        data.Assert(x => x.Assert().HasCountMoreOrExactly(2)).Throws<AssertException>();
        data.Assert(x => x.Assert().HasCountMoreOrExactly(2, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void Contains_Forwarded(int valid, int invalid)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        IEnumerable<int> data = [valid];

        data.Assert().Contains(valid);
        data.Assert().Contains(valid, mod);
        data.Assert(x => x.Assert().Contains(invalid)).Throws<AssertException>();
        data.Assert(x => x.Assert().Contains(invalid, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ContainsAsync_Forwarded(
        AsyncDataSample valid,
        AsyncDataSample invalid
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        IEnumerable<AsyncDataSample> data = [valid];

        await data.Assert().ContainsAsync(valid, canceler);
        await data.Assert().ContainsAsync(valid, canceler, mod);
        await data.Assert()
            .ContainsAsync(invalid, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ContainsAsync(invalid, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void ContainsNot_Forwarded(int valid, int invalid)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        IEnumerable<int> data = [invalid];

        data.Assert().ContainsNot(valid);
        data.Assert().ContainsNot(valid, mod);
        data.Assert(x => x.Assert().ContainsNot(invalid)).Throws<AssertException>();
        data.Assert(x => x.Assert().ContainsNot(invalid, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ContainsNotAsync_Forwarded(
        AsyncDataSample valid,
        AsyncDataSample invalid
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        IEnumerable<AsyncDataSample> data = [invalid];

        await data.Assert().ContainsNotAsync(valid, canceler);
        await data.Assert().ContainsNotAsync(valid, canceler, mod);
        await data.Assert()
            .ContainsNotAsync(invalid, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await data.Assert()
            .ContainsNotAsync(invalid, canceler, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void Fail_Forwarded(IEnumerable<int> data, [Fake] IAsserter asserter)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        ToolSet silentFailSet = MakeSet(asserter);

        data.Assert(silentFailSet).Fail();
        data.Assert(silentFailSet).Fail(mod);
        data.Assert(x => x.Assert().Fail()).Throws<AssertException>();
        data.Assert(x => x.Assert().Fail(mod)).Throws<AssertException>();

        modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal static void Debug_Forwarded(IEnumerable<int> data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        ToolSet debugPassSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = false })
        );
        ToolSet debugFailSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = true })
        );

        data.Assert(debugPassSet).Debug();
        data.Assert(debugPassSet).Debug(mod);
        data.Assert(x => x.Assert(debugFailSet).Debug()).Throws<AssertException>();
        data.Assert(x => x.Assert(debugFailSet).Debug(mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
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
