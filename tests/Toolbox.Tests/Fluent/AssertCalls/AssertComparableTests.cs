using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertComparableTests
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

    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertComparableTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

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
        string[] allowedResults =
        [
            "AssertChainer<AssertComparable>",
            "ResultChainer<",
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
            .Where(r => r.Result as string != nameof(AssertComparable))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void GreaterThan_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        data.Assert().GreaterThan(less);
        data.Assert().GreaterThan(less, _mod);
        data.Assert(d => d.Assert().GreaterThan(same)).Throws<AssertException>();
        data.Assert(d => d.Assert().GreaterThan(more, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void GreaterThanOrEqualTo_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        data.Assert().GreaterThanOrEqualTo(less);
        data.Assert().GreaterThanOrEqualTo(same, _mod);
        data.Assert(d => d.Assert().GreaterThanOrEqualTo(more)).Throws<AssertException>();
        data.Assert(d => d.Assert().GreaterThanOrEqualTo(more, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void GreaterThanOrIs_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        data.Assert().GreaterThanOrIs(less);
        data.Assert().GreaterThanOrIs(same, _mod);
        data.Assert(d => d.Assert().GreaterThanOrIs(more)).Throws<AssertException>();
        data.Assert(d => d.Assert().GreaterThanOrIs(more, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void LessThan_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        data.Assert().LessThan(more);
        data.Assert().LessThan(more, _mod);
        data.Assert(d => d.Assert().LessThan(same)).Throws<AssertException>();
        data.Assert(d => d.Assert().LessThan(less, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void LessThanOrEqualTo_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        data.Assert().LessThanOrEqualTo(more);
        data.Assert().LessThanOrEqualTo(same, _mod);
        data.Assert(d => d.Assert().LessThanOrEqualTo(less)).Throws<AssertException>();
        data.Assert(d => d.Assert().LessThanOrEqualTo(less, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void LessThanOrIs_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        data.Assert().LessThanOrIs(more);
        data.Assert().LessThanOrIs(same, _mod);
        data.Assert(d => d.Assert().LessThanOrIs(less)).Throws<AssertException>();
        data.Assert(d => d.Assert().LessThanOrIs(less, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void InRange_Forwarded(
        [Cap(3, 5)] int data,
        [Copy] int same,
        [Cap(6, 8)] int more,
        [Cap(0, 2)] int less
    )
    {
        data.Assert().InRange(less, same);
        data.Assert().InRange(same, more, _mod);
        data.Assert(d => d.Assert().InRange(less, less)).Throws<AssertException>();
        data.Assert(d => d.Assert().InRange(more, more, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }
}
