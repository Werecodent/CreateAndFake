using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertComparableTests
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
            ],
        };

    [Fact]
    internal static Task AssertComparable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertComparable>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertComparable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertComparable>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertComparable_CallsAndChains(Injected<AssertComparable> instance)
    {
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertComparable>)
            .Where(r => r.Result as string != nameof(AssertComparable))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal static void GreaterThan_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().GreaterThan(less);
        data.Assert().GreaterThan(less, mod);
        data.Assert(d => d.Assert().GreaterThan(same)).Throws<AssertException>();
        data.Assert(d => d.Assert().GreaterThan(more, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void GreaterThanOrEqualTo_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().GreaterThanOrEqualTo(less);
        data.Assert().GreaterThanOrEqualTo(same, mod);
        data.Assert(d => d.Assert().GreaterThanOrEqualTo(more)).Throws<AssertException>();
        data.Assert(d => d.Assert().GreaterThanOrEqualTo(more, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void GreaterThanOrIs_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().GreaterThanOrIs(less);
        data.Assert().GreaterThanOrIs(same, mod);
        data.Assert(d => d.Assert().GreaterThanOrIs(more)).Throws<AssertException>();
        data.Assert(d => d.Assert().GreaterThanOrIs(more, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void LessThan_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().LessThan(more);
        data.Assert().LessThan(more, mod);
        data.Assert(d => d.Assert().LessThan(same)).Throws<AssertException>();
        data.Assert(d => d.Assert().LessThan(less, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void LessThanOrEqualTo_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().LessThanOrEqualTo(more);
        data.Assert().LessThanOrEqualTo(same, mod);
        data.Assert(d => d.Assert().LessThanOrEqualTo(less)).Throws<AssertException>();
        data.Assert(d => d.Assert().LessThanOrEqualTo(less, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void LessThanOrIs_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().LessThanOrIs(more);
        data.Assert().LessThanOrIs(same, mod);
        data.Assert(d => d.Assert().LessThanOrIs(less)).Throws<AssertException>();
        data.Assert(d => d.Assert().LessThanOrIs(less, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void InRange_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().InRange(less, same);
        data.Assert().InRange(same, more, mod);
        data.Assert(d => d.Assert().InRange(less, less)).Throws<AssertException>();
        data.Assert(d => d.Assert().InRange(more, more, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }
}
