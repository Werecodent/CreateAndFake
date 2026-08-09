namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class FakeAttributeTests
{
    [Test]
    public static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }

    [Test]
    public static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }
}
