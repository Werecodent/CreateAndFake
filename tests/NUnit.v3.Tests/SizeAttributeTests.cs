using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class SizeAttributeTests
{
    private static readonly ParameterInfo _SizeParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(SizeAttribute)));

    [Test, RandomData]
    public static Task SizeAttribute_GuardsNulls([Cap(1, 3)] int min, [Cap(3, 5)] int max)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new SizeAttribute(min, max),
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_SizeParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Test, RandomData]
    public static Task SizeAttribute_NoParameterMutation([Cap(1, 3)] int min, [Cap(3, 5)] int max)
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            new SizeAttribute(min, max),
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_SizeParam, IntegrationTests.AttributeMethod] }
        );
    }
}
