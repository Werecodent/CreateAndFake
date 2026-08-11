using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class FakeAttributeTests
{
    private static readonly ParameterInfo _FakeParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(FakeAttribute)));

    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_FakeParam, IntegrationTests.AttributeMethod] }
        );
    }

    [TestMethod]
    public Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_FakeParam, IntegrationTests.AttributeMethod] }
        );
    }
}
