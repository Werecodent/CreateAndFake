using System.Reflection;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskAssertAsyncUnwrapping;

public sealed class TaskAssertAsyncObjectExtensionsTests
{
    private int _modCount;

    private readonly AsserterMod _mod;

    public TaskAssertAsyncObjectExtensionsTests()
    {
        _modCount = 0;
        _mod = opt =>
        {
            _modCount++;
            return opt;
        };
    }

    [Fact]
    internal static Task TaskAssertAsyncObjectExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAssertAsyncObjectExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskAssertAsyncObjectExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAssertAsyncObjectExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static void TaskAssertAsyncObjectExtensions_MatchesEveryMethod()
    {
        typeof(AssertAsyncObjectBase<>)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .OrderBy(m => m.Name)
            .Select(m => m.Name)
            .Assert()
            .Is(
                typeof(TaskAssertAsyncObjectExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .OrderBy(m => m.Name)
                    .Select(m => m.Name)
                    .Where(m => m != nameof(AssertObjectBase<>.Inherits))
                    .Where(m => m != nameof(AssertObjectBase<>.InheritedBy))
            );
    }

    [Theory, RandomData]
    internal async Task IsAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).IsAsync(clone, canceler);
        await Task.FromResult(data.Assert()).IsAsync(clone, canceler, _mod);
        await Task.FromResult(data.Assert())
            .IsAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsAsync(variant, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task IsNotAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).IsNotAsync(variant, canceler);
        await Task.FromResult(data.Assert()).IsNotAsync(variant, canceler, _mod);
        await Task.FromResult(data.Assert())
            .IsNotAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .IsNotAsync(clone, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ValuesEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).ValuesEqualAsync(clone, canceler);
        await Task.FromResult(data.Assert()).ValuesEqualAsync(clone, canceler, _mod);
        await Task.FromResult(data.Assert())
            .ValuesEqualAsync(variant, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesEqualAsync(variant, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task ValuesNotEqualAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        AsyncDataSample variant
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).ValuesNotEqualAsync(variant, canceler);
        await Task.FromResult(data.Assert()).ValuesNotEqualAsync(variant, canceler, _mod);
        await Task.FromResult(data.Assert())
            .ValuesNotEqualAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .ValuesNotEqualAsync(clone, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task UniqueFromAsync_Forwarded(
        AsyncDataSample data,
        [Copy] AsyncDataSample clone,
        [Unique] AsyncDataSample unique
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(data.Assert()).UniqueFromAsync(unique, canceler);
        await Task.FromResult(data.Assert()).UniqueFromAsync(unique, canceler, _mod);
        await Task.FromResult(data.Assert())
            .UniqueFromAsync(clone, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(data.Assert())
            .UniqueFromAsync(clone, canceler, _mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task Inherits_Forwarded([Fake] IParentType parent, [Fake] IChildType child)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await Task.FromResult(child.Assert()).Inherits<IParentType>();
        await Task.FromResult(child.Assert()).Inherits<IParentType>(_mod);
        await Task.FromResult(parent.Assert())
            .Inherits<IChildType>()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(parent.Assert())
            .Inherits<IChildType>(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }

    [Theory, RandomData]
    internal async Task InheritedBy_Forwarded([Fake] IChildType child)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        Type parentType = typeof(IParentType);

        await Task.FromResult(parentType.Assert()).InheritedBy<IChildType>();
        await Task.FromResult(parentType.Assert()).InheritedBy<IChildType>(_mod);
        await Task.FromResult(child.Assert())
            .InheritedBy<IParentType>()
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
        await Task.FromResult(child.Assert())
            .InheritedBy<IParentType>(_mod)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        _modCount.Assert().Is(2);
    }
}
