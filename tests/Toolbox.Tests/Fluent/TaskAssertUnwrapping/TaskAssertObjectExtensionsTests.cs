using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertUnwrapping;

public sealed class TaskAssertObjectExtensionsTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertObjectExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

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
            .Distinct()
            .Assert()
            .Is(
                typeof(TaskAssertObjectExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
                    .Distinct()
            );
    }

    [Theory, RandomData]
    internal async Task Is_Forwarded(DataSample data, [Copy] DataSample clone, DataSample variant)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).Is(clone);
        await Task.FromResult(data.Assert()).Is(clone, _mod);
        await Task.FromResult(data.Assert())
            .Is(variant)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Is(variant, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task IsNull_Forwarded(DataSample data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(((object)null).Assert()).IsNull();
        await Task.FromResult(((object)null).Assert()).IsNull(_mod);
        await Task.FromResult(data.Assert())
            .IsNull()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsNull(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task IsNot_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).IsNot(variant);
        await Task.FromResult(data.Assert()).IsNot(variant, _mod);
        await Task.FromResult(data.Assert())
            .IsNot(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsNot(clone, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task IsNotNull_Forwarded(DataSample data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).IsNotNull();
        await Task.FromResult(data.Assert()).IsNotNull(_mod);
        await Task.FromResult(((object)null).Assert())
            .IsNotNull()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(((object)null).Assert())
            .IsNotNull(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ReferenceEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).ReferenceEqual(data);
        await Task.FromResult(data.Assert()).ReferenceEqual(data, _mod);
        await Task.FromResult(data.Assert())
            .ReferenceEqual(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ReferenceEqual(clone, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ReferenceNotEqual_Forwarded(DataSample data, [Copy] DataSample clone)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).ReferenceNotEqual(clone);
        await Task.FromResult(data.Assert()).ReferenceNotEqual(clone, _mod);
        await Task.FromResult(data.Assert())
            .ReferenceNotEqual(data)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ReferenceNotEqual(data, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ValuesEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).ValuesEqual(clone);
        await Task.FromResult(data.Assert()).ValuesEqual(clone, _mod);
        await Task.FromResult(data.Assert())
            .ValuesEqual(variant)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesEqual(variant, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ValuesNotEqual_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        DataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).ValuesNotEqual(variant);
        await Task.FromResult(data.Assert()).ValuesNotEqual(variant, _mod);
        await Task.FromResult(data.Assert())
            .ValuesNotEqual(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesNotEqual(clone, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task UniqueFrom_Forwarded(
        DataSample data,
        [Copy] DataSample clone,
        [Unique] DataSample unique
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).UniqueFrom(unique);
        await Task.FromResult(data.Assert()).UniqueFrom(unique, _mod);
        await Task.FromResult(data.Assert())
            .UniqueFrom(clone)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .UniqueFrom(clone, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task Inherits_Forwarded([Fake] IParentType parent, [Fake] IChildType child)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(child.Assert()).Inherits(typeof(IParentType));
        await Task.FromResult(child.Assert()).Inherits(typeof(IParentType), _mod);
        await Task.FromResult(parent.Assert())
            .Inherits(typeof(IChildType))
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(parent.Assert())
            .Inherits(typeof(IChildType), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task InheritedBy_Forwarded([Fake] IChildType child)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        Type parentType = typeof(IParentType);

        await Task.FromResult(parentType.Assert()).InheritedBy(typeof(IChildType));
        await Task.FromResult(parentType.Assert()).InheritedBy(typeof(IChildType), _mod);
        await Task.FromResult(child.Assert())
            .InheritedBy(typeof(IParentType))
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(child.Assert())
            .InheritedBy(typeof(IParentType), _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task Fail_Forwarded(DataSample data, [Fake] IAsserter asserter)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        ToolSet silentFailSet = MakeSet(asserter);

        await Task.FromResult(data.Assert(silentFailSet)).Fail();
        await Task.FromResult(data.Assert(silentFailSet)).Fail(_mod);
        await Task.FromResult(data.Assert()).Fail().Assert().ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .Fail(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal async Task Debug_Forwarded(DataSample data)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        ToolSet debugPassSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = false })
        );
        ToolSet debugFailSet = MakeSet(
            Tools.Asserter.WithOptions(opt => opt with { DebugAssertsFail = true })
        );

        await Task.FromResult(data.Assert(debugPassSet)).Debug();
        await Task.FromResult(data.Assert(debugPassSet)).Debug(_mod);
        await Task.FromResult(data.Assert(debugFailSet))
            .Debug()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert(debugFailSet))
            .Debug(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task Pass_Forwarded(DataSample data)
    {
        await Task.FromResult(data.Assert()).Pass();
        await Task.FromResult(data.Assert()).Pass(_mod);

        _modCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal async Task Called_Forwarded([Fake] DataSample data, DataSample nonFake)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        data.ToString();

        await Task.FromResult(data.Assert()).Called();
        await Task.FromResult(data.Assert()).Called(_mod);
        await Task.FromResult(data.Assert()).Called(Times.Once);
        await Task.FromResult(data.Assert()).Called(Times.Once, _mod);

        data.Equals(Arg.Any<object>()).SetupReturn(Behavior<bool>.Throw());

        await Task.FromResult(data.Assert())
            .Called()
            .Assert()
            .ThrowsAsync<FakeVerifyException>(canceler);
        await Task.FromResult(data.Assert())
            .Called(_mod)
            .Assert()
            .ThrowsAsync<FakeVerifyException>(canceler);
        await Task.FromResult(data.Assert())
            .Called(Times.Never)
            .Assert()
            .ThrowsAsync<FakeVerifyException>(canceler);
        await Task.FromResult(data.Assert())
            .Called(Times.Never, _mod)
            .Assert()
            .ThrowsAsync<FakeVerifyException>(canceler);

        await Task.FromResult(nonFake.Assert())
            .Called()
            .Assert()
            .ThrowsAsync<ToolException>(canceler);
        await Task.FromResult(nonFake.Assert())
            .Called(_mod)
            .Assert()
            .ThrowsAsync<ToolException>(canceler);
        await Task.FromResult(nonFake.Assert())
            .Called(Times.Any)
            .Assert()
            .ThrowsAsync<ToolException>(canceler);
        await Task.FromResult(nonFake.Assert())
            .Called(Times.Any, _mod)
            .Assert()
            .ThrowsAsync<ToolException>(canceler);

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
