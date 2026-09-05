using Werecodent.CreateAndFake.Samples.DoubleValue;

namespace Werecodent.CreateAndFake.Samples.Tests.DoubleValue;

public static class WriteableHolderTests
{
    [Fact]
    public static Task WriteableHolder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(WriteableHolder<,>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task WriteableHolder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(WriteableHolder<,>),
            TestContext.Current.CancellationToken
        );
    }
}
