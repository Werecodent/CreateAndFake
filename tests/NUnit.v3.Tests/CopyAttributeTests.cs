using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class CopyAttributeTests
{
    private static readonly ParameterInfo _CopyParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(CopyAttribute)));

    [Test]
    public static Task CopyAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CopyAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_CopyParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Test]
    public static Task CopyAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CopyAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_CopyParam, IntegrationTests.AttributeMethod] }
        );
    }
}
