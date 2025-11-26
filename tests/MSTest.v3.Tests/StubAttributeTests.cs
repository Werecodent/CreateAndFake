namespace CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class StubAttributeTests
{
    [TestMethod]
    public Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<StubAttribute>();
    }

    [TestMethod]
    public Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<StubAttribute>();
    }
}
