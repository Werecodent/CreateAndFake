using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class AsyncEnumHelperTests
{
    [Fact]
    internal static Task AsyncEnumHelper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(AsyncEnumHelper),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsyncEnumHelper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(AsyncEnumHelper),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static async Task CreateFrom_ConvertsObjectsSuccessfully(IList<string> data)
    {
        int i = 0;
        await foreach (
            string value in AsyncEnumHelper
                .CreateFrom(data)
                .WithCancellation(TestContext.Current.CancellationToken)
        )
        {
            value.Assert().Is(data[i++]);
        }
        i.Assert().Is(data.Count);
    }
}
