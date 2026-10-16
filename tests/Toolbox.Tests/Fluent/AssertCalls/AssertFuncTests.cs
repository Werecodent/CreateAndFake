using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertFuncTests
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

    public AssertFuncTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task AssertFunc_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertFunc<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertFunc_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertFunc<string>>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertFunc_CallsAndChains(Injected<AssertFunc<string>> instance)
    {
        string[] allowedResults =
        [
            "AssertChainer<AssertFunc<",
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
            .Where(r => r.Result as string != "AssertFunc<String>")
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void HasResult_Forwarded(Func<string> behavior, Exception error)
    {
        Func<string> thrower = () => throw error;

        behavior.Assert().HasResult();
        behavior.Assert().HasResult(_mod);

        thrower.Assert(x => x.Assert().HasResult()).Throws<AssertException>();
        thrower.Assert(x => x.Assert().HasResult(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }
}
