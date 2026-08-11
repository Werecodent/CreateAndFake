using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public static class CapAttribute_T_Tests
{
    private static readonly ParameterInfo _CapParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CapAttribute<double>)));

    [Fact]
    internal static Task CapAttribute_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CapAttribute<double>>(
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
    internal static Task CapAttribute_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CapAttribute<double>>(
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
