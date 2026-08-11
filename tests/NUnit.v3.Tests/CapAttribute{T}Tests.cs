using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class CapAttribute_T_Tests
{
    private static readonly ParameterInfo _CapParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CapAttribute<double>)));

    [Test]
    public static Task CapAttribute_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(CapAttribute<>),
            TestContext.CurrentContext.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [_CapParam, IntegrationTests.AttributeMethod],
                    IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                }
        );
    }

    [Test]
    public static Task CapAttribute_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CapAttribute<double>>(
            TestContext.CurrentContext.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [_CapParam, IntegrationTests.AttributeMethod],
                    IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                }
        );
    }
}
