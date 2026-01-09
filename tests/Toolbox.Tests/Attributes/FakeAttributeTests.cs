namespace CreateAndFake.Tests.Attributes;

public static class FakeAttributeTests
{
    [Fact]
    internal static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
