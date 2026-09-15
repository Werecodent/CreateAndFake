using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public static class TaskAssertObjectExtensionsTests
{
    [Fact]
    internal static Task TaskAssertObjectExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertObjectExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ToolException)],
                }
        );
    }

    [Fact]
    internal static Task TaskAssertObjectExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertObjectExtensions),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(AssertException), typeof(ToolException)],
                }
        );
    }

    [Fact]
    internal static void TaskAssertObjectExtensions_MatchesEveryMethod()
    {
        typeof(AssertObjectBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Where(x => x != nameof(ToString))
            .Assert()
            .Is(
                typeof(TaskAssertObjectExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
            );
    }

    [Theory, RandomData]
    internal static async Task Is_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).Is(clone);
        await Task.FromResult(data.Assert()).Is(clone, mod);
        await Task.FromResult(data.Assert())
            .Is(variant)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Is(variant, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task IsNull_Forwarded(DataSample data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(((object)null).Assert()).IsNull();
        await Task.FromResult(((object)null).Assert()).IsNull(mod);
        await Task.FromResult(data.Assert())
            .IsNull()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsNull(mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task IsNot_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).IsNot(variant);
        await Task.FromResult(data.Assert()).IsNot(variant, mod);
        await Task.FromResult(data.Assert())
            .IsNot(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsNot(clone, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task IsNotNull_Forwarded(DataSample data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).IsNotNull();
        await Task.FromResult(data.Assert()).IsNotNull(mod);
        await Task.FromResult(((object)null).Assert())
            .IsNotNull()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(((object)null).Assert())
            .IsNotNull(mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ReferenceEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).ReferenceEqual(data);
        await Task.FromResult(data.Assert()).ReferenceEqual(data, mod);
        await Task.FromResult(data.Assert())
            .ReferenceEqual(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ReferenceEqual(clone, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ReferenceNotEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).ReferenceNotEqual(clone);
        await Task.FromResult(data.Assert()).ReferenceNotEqual(clone, mod);
        await Task.FromResult(data.Assert())
            .ReferenceNotEqual(data)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ReferenceNotEqual(data, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ValuesEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).ValuesEqual(clone);
        await Task.FromResult(data.Assert()).ValuesEqual(clone, mod);
        await Task.FromResult(data.Assert())
            .ValuesEqual(variant)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesEqual(variant, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task ValuesNotEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).ValuesNotEqual(variant);
        await Task.FromResult(data.Assert()).ValuesNotEqual(variant, mod);
        await Task.FromResult(data.Assert())
            .ValuesNotEqual(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesNotEqual(clone, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task UniqueFrom_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        [Unique] DataSample unique
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).UniqueFrom(unique);
        await Task.FromResult(data.Assert()).UniqueFrom(unique, mod);
        await Task.FromResult(data.Assert())
            .UniqueFrom(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .UniqueFrom(clone, mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task Fail_Forwarded(DataSample data, [Fake] IAsserter asserter)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }
        ToolSet silentFailSet = MakeSet(asserter);

        await Task.FromResult(data.Assert(silentFailSet)).Fail();
        await Task.FromResult(data.Assert(silentFailSet)).Fail(mod);
        await Task.FromResult(data.Assert()).Fail().Assert().ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Fail(mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal static async Task Debug_Forwarded(DataSample data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
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

        await Task.FromResult(data.Assert(debugPassSet)).Debug();
        await Task.FromResult(data.Assert(debugPassSet)).Debug(mod);
        await Task.FromResult(data.Assert(debugFailSet))
            .Debug()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert(debugFailSet))
            .Debug(mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static async Task Pass_Forwarded(DataSample data)
    {
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        await Task.FromResult(data.Assert()).Pass();
        await Task.FromResult(data.Assert()).Pass(mod);

        modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal static async Task Called_Forwarded([Fake] DataSample data, DataSample nonFake)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;
        int modCount = 0;
        AsserterOptions mod(AsserterOptions opt)
        {
            modCount++;
            return opt;
        }

        data.ToString();

        await Task.FromResult(data.Assert()).Called();
        await Task.FromResult(data.Assert()).Called(mod);

        data.Equals(Arg.Any<object>()).SetupReturn(Behavior<bool>.Throw());

        await Task.FromResult(data.Assert())
            .Called()
            .Assert()
            .ThrowsAsync<FakeVerifyException>(canceler);
        await Task.FromResult(data.Assert())
            .Called(mod)
            .Assert()
            .ThrowsAsync<FakeVerifyException>(canceler);

        await Task.FromResult(nonFake.Assert())
            .Called()
            .Assert()
            .ThrowsAsync<ToolException>(canceler);
        await Task.FromResult(nonFake.Assert())
            .Called(mod)
            .Assert()
            .ThrowsAsync<ToolException>(canceler);

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
