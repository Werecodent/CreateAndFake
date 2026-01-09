namespace CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class StubAttributeTests
{
    [Test]
    public static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<StubAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }

    [Test]
    public static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<StubAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }
}
