using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class CopyAttributeTests
{
    private static readonly ParameterInfo _CopyParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CopyAttribute)));

    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task CopyAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CopyAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_CopyParam, IntegrationTests.AttributeMethod] }
        );
    }

    [TestMethod]
    public Task CopyAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CopyAttribute>(
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_CopyParam, IntegrationTests.AttributeMethod] }
        );
    }
}
