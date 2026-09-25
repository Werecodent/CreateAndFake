using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class InjectAttributeTests
{
    private static readonly ParameterInfo _InjectParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(InjectAttribute)));

    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task InjectAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InjectAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }

    [TestMethod]
    public Task InjectAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InjectAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }
}
