using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public static class CapAttributeTests
{
    private static readonly ParameterInfo _CapParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CapAttribute)));

    [Fact]
    internal static Task CapAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            new CapAttribute(5, 10),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [_CapParam, IntegrationTests.AttributeMethod],
                    IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                }
        );
    }

    [Fact]
    internal static Task CapAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new CapAttribute(5, 10),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    InjectionValues = [_CapParam, IntegrationTests.AttributeMethod],
                    IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                }
        );
    }
}
