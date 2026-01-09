namespace CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class StubAttributeTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<StubAttribute>(TestContext.CancellationToken);
    }

    [TestMethod]
    public Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<StubAttribute>(TestContext.CancellationToken);
    }
}
