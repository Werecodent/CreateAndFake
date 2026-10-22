using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public static class InjectAttributeTests
{
    private static readonly ParameterInfo _InjectParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(InjectAttribute)));

    [Fact]
    internal static Task InjectAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InjectAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Fact]
    internal static Task InjectAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InjectAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }
}
