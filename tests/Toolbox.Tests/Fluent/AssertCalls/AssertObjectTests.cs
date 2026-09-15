using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertCalls;

public static class AssertObjectTests
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
        RunResults results = await Tools.Runner.CallMethodsOnAsync(
            instance.Dummy,
            TestContext.Current.CancellationToken,
            opt => opt with { IncludeBaseObjectMethods = false }
        );
        results
            .RawResults.Where(r => r.Result != null)
            .Where(r => r.Result is not AssertChainer<AssertObject>)
            .Where(r => r.Result as string != nameof(AssertObject))
            .Assert()
            .IsEmpty();
    }

    [Theory, RandomData]
    internal static void Is_Forwarded(DataSample data, [Copy] DataSample clone, DataSample variant)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().Is(clone);
        data.Assert().Is(clone, mod);
        data.Assert(d => d.Assert().Is(variant)).Throws<AssertException>();
        data.Assert(d => d.Assert().Is(variant, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void IsNull_Forwarded(DataSample data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        ((object)null).Assert().IsNull();
        ((object)null).Assert().IsNull(mod);
        data.Assert(d => d.Assert().IsNull()).Throws<AssertException>();
        data.Assert(d => d.Assert().IsNull(mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void IsNot_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().IsNot(variant);
        data.Assert().IsNot(variant, mod);
        data.Assert(d => d.Assert().IsNot(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().IsNot(clone, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void IsNotNull_Forwarded(DataSample data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().IsNotNull();
        data.Assert().IsNotNull(mod);
        ((object)null).Assert(d => d.Assert().IsNotNull()).Throws<AssertException>();
        ((object)null).Assert(d => d.Assert().IsNotNull(mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void ReferenceEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().ReferenceEqual(data);
        data.Assert().ReferenceEqual(data, mod);
        data.Assert(d => d.Assert().ReferenceEqual(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().ReferenceEqual(clone, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void ReferenceNotEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().ReferenceNotEqual(clone);
        data.Assert().ReferenceNotEqual(clone, mod);
        data.Assert(d => d.Assert().ReferenceNotEqual(data)).Throws<AssertException>();
        data.Assert(d => d.Assert().ReferenceNotEqual(data, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void ValuesEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().ValuesEqual(clone);
        data.Assert().ValuesEqual(clone, mod);
        data.Assert(d => d.Assert().ValuesEqual(variant)).Throws<AssertException>();
        data.Assert(d => d.Assert().ValuesEqual(variant, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void ValuesNotEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().ValuesNotEqual(variant);
        data.Assert().ValuesNotEqual(variant, mod);
        data.Assert(d => d.Assert().ValuesNotEqual(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().ValuesNotEqual(clone, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void UniqueFrom_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        [Unique] DataSample unique
    )
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().UniqueFrom(unique);
        data.Assert().UniqueFrom(unique, mod);
        data.Assert(d => d.Assert().UniqueFrom(clone)).Throws<AssertException>();
        data.Assert(d => d.Assert().UniqueFrom(clone, mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void Fail_Forwarded(DataSample data, [Fake] IAsserter asserter)
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
        data.Assert(d => d.Assert().Fail()).Throws<AssertException>();
        data.Assert(d => d.Assert().Fail(mod)).Throws<AssertException>();

        modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal static void Debug_Forwarded(DataSample data)
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
        data.Assert(d => d.Assert(debugFailSet).Debug()).Throws<AssertException>();
        data.Assert(d => d.Assert(debugFailSet).Debug(mod)).Throws<AssertException>();

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void Pass_Forwarded(DataSample data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.Assert().Pass();
        data.Assert().Pass(mod);

        modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal static void Called_Forwarded([Fake] DataSample data, DataSample nonFake)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.ToString();

        data.Assert().Called();
        data.Assert().Called(mod);

        data.Equals(Arg.Any<object>()).SetupReturn(Behavior<bool>.Throw());

        data.Assert(d => d.Assert().Called()).Throws<FakeVerifyException>();
        data.Assert(d => d.Assert().Called(mod)).Throws<FakeVerifyException>();

        nonFake.Assert(d => d.Assert().Called()).Throws<ToolException>();
        nonFake.Assert(d => d.Assert().Called(mod)).Throws<ToolException>();

        modCount.Assert().Is(0);
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
