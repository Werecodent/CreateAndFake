using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class CapAttributeTests
{
    private static readonly ParameterInfo _CapParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CapAttribute)));

    [Test]
    public static Task CapAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CapAttribute>(
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
    public static Task CapAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CapAttribute>(
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
