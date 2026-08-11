using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public static class StubAttributeTests
{
    private static readonly ParameterInfo _StubParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(StubAttribute)));

    [Fact]
    internal static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StubAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_StubParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Fact]
    internal static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StubAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_StubParam, IntegrationTests.AttributeMethod] }
        );
    }
}
