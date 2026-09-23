using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertActionTests
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

    public AssertActionTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task AssertAction_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertAction>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertAction_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertAction>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertAction_CallsAndChains(Injected<AssertAction> instance)
    {
        string[] allowedResults =
        [
            "AssertChainer<AssertAction>",
            "ExceptionChainer<",
            nameof(AlsoChainer),
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
            .Where(r => r.Result as string != nameof(AssertAction))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void Throws_Forwarded(Action behavior, Exception error)
    {
        Action thrower = () => throw error;

        thrower.Assert().Throws<Exception>();
        thrower.Assert().Throws<Exception>(_mod);
        behavior.Assert(x => x.Assert().Throws<Exception>()).Throws<AssertException>();
        behavior.Assert(x => x.Assert().Throws<Exception>(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ThrowsException_Forwarded(Action behavior, Exception error)
    {
        Action thrower = () => throw error;

        thrower.Assert().ThrowsException();
        thrower.Assert().ThrowsException(_mod);
        behavior.Assert(x => x.Assert().ThrowsException()).Throws<AssertException>();
        behavior.Assert(x => x.Assert().ThrowsException(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ThrowsNo_Forwarded(Action behavior, Exception error)
    {
        Action thrower = () => throw error;

        behavior.Assert().ThrowsNo<Exception>();
        behavior.Assert().ThrowsNo<Exception>(_mod);
        thrower.Assert(x => x.Assert().ThrowsNo<Exception>()).Throws<AssertException>();
        thrower.Assert(x => x.Assert().ThrowsNo<Exception>(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ThrowsNoException_Forwarded(Action behavior, Exception error)
    {
        Action thrower = () => throw error;

        behavior.Assert().ThrowsNoException();
        behavior.Assert().ThrowsNoException(_mod);
        thrower.Assert(x => x.Assert().ThrowsNoException()).Throws<AssertException>();
        thrower.Assert(x => x.Assert().ThrowsNoException(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }
}
