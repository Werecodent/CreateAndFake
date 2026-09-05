using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class CapAttributeTests
{
    private static readonly ParameterInfo _CapParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CapAttribute)));

    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task CapAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new CapAttribute(5, 10),
            TestContext.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [_CapParam, IntegrationTests.AttributeMethod],
                    IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                }
        );
    }

    [TestMethod]
    public Task CapAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new CapAttribute(5, 10),
            TestContext.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [_CapParam, IntegrationTests.AttributeMethod],
                    IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                }
        );
    }
}
