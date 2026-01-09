namespace CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class FakeAttributeTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>(TestContext.CancellationToken);
    }

    [TestMethod]
    public Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>(TestContext.CancellationToken);
    }
}
