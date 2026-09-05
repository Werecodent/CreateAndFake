using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.Tests.SingleValue;

public static class ReadableHolderTests
{
    [Fact]
    public static Task ReadableHolder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ReadableHolder<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ReadableHolder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ReadableHolder<>),
            TestContext.Current.CancellationToken
        );
    }
}
