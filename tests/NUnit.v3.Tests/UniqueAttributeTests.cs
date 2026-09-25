using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class UniqueAttributeTests
{
    private static readonly ParameterInfo _UniqueParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(UniqueAttribute)));

    [Test]
    public static Task UniqueAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<UniqueAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Test]
    public static Task UniqueAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<UniqueAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_UniqueParam, IntegrationTests.AttributeMethod] }
        );
    }
}
