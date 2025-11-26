namespace CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class SizeAttributeTests
{
    [TestMethod]
    public Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>();
    }

    [TestMethod]
    public Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>();
    }
}
