using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public static class UniqueAttributeTests
{
    private static readonly ParameterInfo _UniqueParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(UniqueAttribute)));

    [Fact]
    internal static Task UniqueAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<UniqueAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Fact]
    internal static Task UniqueAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<UniqueAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }
}
