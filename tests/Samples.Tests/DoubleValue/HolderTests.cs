using Werecodent.CreateAndFake.Samples.DoubleValue;

namespace Werecodent.CreateAndFake.Samples.Tests.DoubleValue;

public static class HolderTests
{
    [Fact]
    public static Task Holder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Holder<,>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task Holder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Holder<,>),
            TestContext.Current.CancellationToken
        );
    }
}
