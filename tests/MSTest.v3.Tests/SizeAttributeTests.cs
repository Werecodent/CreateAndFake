using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class SizeAttributeTests
{
    private static readonly ParameterInfo _SizeParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(SizeAttribute)));

    public TestContext TestContext { get; set; }

    [TestMethod, RandomData]
    public Task SizeAttribute_GuardsNulls([Cap(1, 3)] int min, [Cap(3, 5)] int max)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new SizeAttribute(min, max),
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_SizeParam, IntegrationTests.AttributeMethod] }
        );
    }

    [TestMethod, RandomData]
    public Task SizeAttribute_NoParameterMutation([Cap(1, 3)] int min, [Cap(3, 5)] int max)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new SizeAttribute(min, max),
            TestContext.CancellationToken,
            opt => opt with { InjectionValues = [_SizeParam, IntegrationTests.AttributeMethod] }
        );
    }
}
