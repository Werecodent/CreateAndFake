using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class StubAttributeTests
{
    private static readonly ParameterInfo _StubParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(StubAttribute)));

    [Test]
    public static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StubAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_StubParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Test]
    public static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StubAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_StubParam, IntegrationTests.AttributeMethod] }
        );
    }
}
