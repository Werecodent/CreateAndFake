using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class CopyAttributeTests
{
    private static readonly ParameterInfo _CopyParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CopyAttribute)));

    [Fact]
    internal static Task CopyAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CopyAttribute>(
            CancellationToken.None,
            opt => opt with { InjectionValues = [_CopyParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Fact]
    internal static Task CopyAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CopyAttribute>(
            CancellationToken.None,
            opt => opt with { InjectionValues = [_CopyParam, IntegrationTests.AttributeMethod] }
        );
    }
}
