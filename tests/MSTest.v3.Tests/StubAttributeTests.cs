using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class StubAttributeTests
{
    private static readonly ParameterInfo _StubParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(StubAttribute)));

    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StubAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_StubParam, IntegrationTests.AttributeMethod] }
        );
    }

    [TestMethod]
    public Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StubAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_StubParam, IntegrationTests.AttributeMethod] }
        );
    }
}
