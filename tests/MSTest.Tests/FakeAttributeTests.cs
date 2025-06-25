namespace CreateAndFake.MSTest.Tests;

[TestClass]
public class FakeAttributeTests
{
    [TestMethod]
    public Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>();
    }

    [TestMethod]
    public Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>();
    }
}
