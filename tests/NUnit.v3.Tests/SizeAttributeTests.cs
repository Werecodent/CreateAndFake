namespace CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class SizeAttributeTests
{
    [Test]
    public static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }

    [Test]
    public static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }
}
