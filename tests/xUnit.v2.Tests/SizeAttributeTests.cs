using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class SizeAttributeTests
{
    private static readonly ParameterInfo _SizeParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(SizeAttribute)));

    [Theory, RandomData]
    internal static Task SizeAttribute_GuardsNulls([Cap(1, 3)] int min, [Cap(3, 5)] int max)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new SizeAttribute(min, max),
            CancellationToken.None,
            opt => opt with { InjectionValues = [_SizeParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Theory, RandomData]
    internal static Task SizeAttribute_NoParameterMutation([Cap(1, 3)] int min, [Cap(3, 5)] int max)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new SizeAttribute(min, max),
            CancellationToken.None,
            opt => opt with { InjectionValues = [_SizeParam, IntegrationTests.AttributeMethod] }
        );
    }
}
