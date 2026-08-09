using Werecodent.CreateAndFake.Samples.BasicData;

namespace Werecodent.CreateAndFake.Samples.Tests.BasicData;

public static class CollectionDtoTests
{
    [Fact]
    public static Task CollectionDto_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CollectionDto>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task CollectionDto_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CollectionDto>(
            TestContext.Current.CancellationToken
        );
    }
}
