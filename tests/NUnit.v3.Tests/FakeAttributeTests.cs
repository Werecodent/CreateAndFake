using System.Reflection;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class FakeAttributeTests
{
    private static readonly ParameterInfo _FakeParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(FakeAttribute)));

    [Test]
    public static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_FakeParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Test]
    public static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(
            TestContext.CurrentContext.CancellationToken,
            opt => opt with { InjectionValues = [_FakeParam, IntegrationTests.AttributeMethod] }
        );
    }
}
