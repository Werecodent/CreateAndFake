using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class InjectAttributeTests
{
    private static readonly ParameterInfo _InjectParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(InjectAttribute)));

    [Fact]
    internal static Task InjectAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InjectAttribute>(
            CancellationToken.None,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Fact]
    internal static Task InjectAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InjectAttribute>(
            CancellationToken.None,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }
}
