using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class InjectAttributeTests
{
    private static readonly ParameterInfo _InjectParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(InjectAttribute)));

    [Test]
    public static Task InjectAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InjectAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Test]
    public static Task InjectAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InjectAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_InjectParam, IntegrationTests.AttributeMethod] }
        );
    }
}
