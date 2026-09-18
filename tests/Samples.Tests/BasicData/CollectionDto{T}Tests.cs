using Werecodent.CreateAndFake.Samples.BasicData;

namespace Werecodent.CreateAndFake.Samples.Tests.BasicData;

public static class CollectionDto_T_Tests
{
    [Fact]
    public static Task CollectionDto_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(CollectionDto<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task CollectionDto_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(CollectionDto<>),
            TestContext.Current.CancellationToken
        );
    }
}
