using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.Tests.SingleValue;

public static class HolderTests
{
    [Fact]
    public static Task Holder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Holder<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task Holder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Holder<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task Holder_GuardsNullsWithDisposable()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Holder<SelfReferencingDisposable>>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task Holder_NoParameterMutationWithDisposable()
    {
        return Tools.Tester.PreventsParameterMutationAsync<Holder<SelfReferencingDisposable>>(
            TestContext.Current.CancellationToken
        );
    }
}
