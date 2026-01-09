namespace CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class SizeAttributeTests
{
    [Test]
    public static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }

    [Test]
    public static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }
}
