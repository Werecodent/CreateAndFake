using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class UniqueAttributeTests
{
    private static readonly ParameterInfo _UniqueParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(UniqueAttribute)));

    [Fact]
    internal static Task UniqueAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<UniqueAttribute>(
            CancellationToken.None,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Fact]
    internal static Task UniqueAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<UniqueAttribute>(
            CancellationToken.None,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }
}
