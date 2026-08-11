using System.Reflection;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public static class FakeAttributeTests
{
    private static readonly ParameterInfo _FakeParam = IntegrationTests
        .AttributeMethod.GetParameters()
        .First(p => Attribute.IsDefined(p, typeof(FakeAttribute)));

    [Fact]
    internal static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_FakeParam, IntegrationTests.AttributeMethod] }
        );
    }

    [Fact]
    internal static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [_FakeParam, IntegrationTests.AttributeMethod] }
        );
    }
}
