using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class CapAttributeTests
{
    private static readonly ParameterInfo _CapParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CapAttribute)));

    [Fact]
    internal static Task CapAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CapAttribute>(
            CancellationToken.None,
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
        return Tools.Tester.PreventsParameterMutationAsync<CapAttribute>(
            CancellationToken.None,
            opt =>
                opt with
                {
                    InjectionValues = [_CapParam, IntegrationTests.AttributeMethod],
                    IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                }
        );
    }
}
