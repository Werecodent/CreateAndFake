using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class UniqueAttributeTests
{
    private static readonly ParameterInfo _UniqueParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(UniqueAttribute)));

    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task UniqueAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<UniqueAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }

    [TestMethod]
    public Task UniqueAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<UniqueAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }
}
