using System.Reflection;

namespace Werecodent.CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class CapAttribute_T_Tests
{
    private static readonly ParameterInfo _CapParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CapAttribute<double>)));

    public TestContext TestContext { get; set; }

    [TestMethod]
    public Task CapAttribute_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(CapAttribute<>),
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
    public Task CapAttribute_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CapAttribute<double>>(
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
