namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class StubAttributeTests
{
    [Test]
    public static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StubAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }

    [Test]
    public static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StubAttribute>(
            TestContext.CurrentContext.CancellationToken
        );
    }
}
