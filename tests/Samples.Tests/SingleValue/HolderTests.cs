using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.Tests.SingleValue;

public sealed class HolderTests() : AutoTester(typeof(Holder<>))
{
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
