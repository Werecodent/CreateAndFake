namespace Werecodent.CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class StubAttributeTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StubAttribute>(
            TestContext.CancellationToken
        );
    }

    [TestMethod]
    public Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StubAttribute>(
            TestContext.CancellationToken
        );
    }
}
