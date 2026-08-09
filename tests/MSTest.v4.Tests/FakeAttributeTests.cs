namespace Werecodent.CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class FakeAttributeTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(
            TestContext.CancellationToken
        );
    }

    [TestMethod]
    public Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(
            TestContext.CancellationToken
        );
    }
}
