using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertErrorTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ToolException),
                typeof(InvalidCastException),
            ],
        };

    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertErrorTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task AssertError_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertError>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertError_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertError>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertError_CallsAndChains(Injected<AssertError> instance)
    {
        string[] allowedResults =
        [
            "AssertChainer<AssertError>",
            "ExceptionChainer<",
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
            .Where(r => r.Result as string != nameof(AssertError))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void Inherits_Forwarded(AggregateException error)
    {
        error.Assert().Inherits<Exception>();
        error.Assert().Inherits<Exception>(_mod);
        error
            .Assert(x => x.Assert().Inherits<InvalidOperationException>())
            .Throws<AssertException>();
        error
            .Assert(x => x.Assert().Inherits<InvalidOperationException>(_mod))
            .Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void Fail_Forwarded(Exception data, [Fake] IAsserter asserter)
    {
        ToolSet silentFailSet = MakeSet(asserter);

        data.Assert(silentFailSet).Fail();
        data.Assert(silentFailSet).Fail(_mod);
        data.Assert(x => x.Assert().Fail()).Throws<AssertException>();
        data.Assert(x => x.Assert().Fail(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal void Debug_Forwarded(Exception data)
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
