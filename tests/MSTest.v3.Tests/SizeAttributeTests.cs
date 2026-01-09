namespace CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class SizeAttributeTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>(TestContext.CancellationToken);
    }

    [TestMethod]
    public Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>(TestContext.CancellationToken);
    }
}
