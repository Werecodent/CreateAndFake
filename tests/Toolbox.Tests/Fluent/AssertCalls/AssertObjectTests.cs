using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public sealed class AssertObjectTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ToolException),
                typeof(FakeVerifyException),
                typeof(InvalidCastException),
            ],
        };

    private int _modCount;

    private readonly AsserterMod _mod;

    public AssertObjectTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task AssertObject_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertObject>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task AssertObject_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertObject>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static async Task AssertObject_CallsAndChains(Injected<AssertObject> instance)
    {
        string[] allowedResults = ["AssertChainer<AssertObject>", nameof(VoidType)];

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
            .Where(r => r.Result as string != nameof(AssertObject))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal void Is_Forwarded(DataSample data, [Copy] DataSample clone, DataSample variant)
    {
        data.Assert().Is(clone);
        data.Assert().Is(clone, _mod);
        data.Assert(d => d.Assert().Is(variant)).Throws<AssertException>();
        data.Assert(d => d.Assert().Is(variant, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void IsNull_Forwarded(DataSample data)
    {
        ((object)null).Assert().IsNull();
        ((object)null).Assert().IsNull(_mod);
        data.Assert(d => d.Assert().IsNull()).Throws<AssertException>();
        data.Assert(d => d.Assert().IsNull(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void IsNot_Forwarded(DataSample data, [Copy] DataSample clone, DataSample variant)
    {
        data.Assert().IsNot(variant);
        data.Assert().IsNot(variant, _mod);
        data.Assert(d => d.Assert().IsNot(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().IsNot(clone, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void IsNotNull_Forwarded(DataSample data)
    {
        data.Assert().IsNotNull();
        data.Assert().IsNotNull(_mod);
        ((object)null).Assert(d => d.Assert().IsNotNull()).Throws<AssertException>();
        ((object)null).Assert(d => d.Assert().IsNotNull(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ReferenceEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        data.Assert().ReferenceEqual(data);
        data.Assert().ReferenceEqual(data, _mod);
        data.Assert(d => d.Assert().ReferenceEqual(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().ReferenceEqual(clone, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ReferenceNotEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        data.Assert().ReferenceNotEqual(clone);
        data.Assert().ReferenceNotEqual(clone, _mod);
        data.Assert(d => d.Assert().ReferenceNotEqual(data)).Throws<AssertException>();
        data.Assert(d => d.Assert().ReferenceNotEqual(data, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ValuesEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        data.Assert().ValuesEqual(clone);
        data.Assert().ValuesEqual(clone, _mod);
        data.Assert(d => d.Assert().ValuesEqual(variant)).Throws<AssertException>();
        data.Assert(d => d.Assert().ValuesEqual(variant, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void ValuesNotEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        data.Assert().ValuesNotEqual(variant);
        data.Assert().ValuesNotEqual(variant, _mod);
        data.Assert(d => d.Assert().ValuesNotEqual(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().ValuesNotEqual(clone, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void UniqueFrom_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        [Unique] DataSample unique
    )
    {
        data.Assert().UniqueFrom(unique);
        data.Assert().UniqueFrom(unique, _mod);
        data.Assert(d => d.Assert().UniqueFrom(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().UniqueFrom(clone, _mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void Fail_Forwarded(DataSample data, [Fake] IAsserter asserter)
    {
        ToolSet silentFailSet = MakeSet(asserter);

        data.Assert(silentFailSet).Fail();
        data.Assert(silentFailSet).Fail(_mod);
        data.Assert(d => d.Assert().Fail()).Throws<AssertException>();
        data.Assert(d => d.Assert().Fail(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal void Debug_Forwarded(DataSample data)
    {
        ToolSet debugPassSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = false })
        );
        ToolSet debugFailSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = true })
        );

        data.Assert(debugPassSet).Debug();
        data.Assert(debugPassSet).Debug(_mod);
        data.Assert(d => d.Assert(debugFailSet).Debug()).Throws<AssertException>();
        data.Assert(d => d.Assert(debugFailSet).Debug(_mod)).Throws<AssertException>();

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal void Pass_Forwarded(DataSample data)
    {
        data.Assert().Pass();
        data.Assert().Pass(_mod);

        _modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal void Called_Forwarded([Fake] DataSample data, DataSample nonFake)
    {
        data.ToString();

        data.Assert().Called();
        data.Assert().Called(_mod);

        data.Equals(Arg.Any<object>()).SetupReturn(Behavior<bool>.Throw());

        data.Assert(d => d.Assert().Called()).Throws<FakeVerifyException>();
        data.Assert(d => d.Assert().Called(_mod)).Throws<FakeVerifyException>();

        nonFake.Assert(d => d.Assert().Called()).Throws<ToolException>();
        nonFake.Assert(d => d.Assert().Called(_mod)).Throws<ToolException>();

        _modCount.Assert().Is(0);
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
