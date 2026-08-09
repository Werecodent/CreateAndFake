using System.Reflection;

namespace Werecodent.CreateAndFake.Design.Tests.Properties;

public static class CallerArgumentExpressionAttributeTests
{
    private static readonly Type _TestType = Assembly
        .GetAssembly(typeof(ArgumentGuard))
        .GetType("CallerArgumentExpressionAttribute");

    [Fact]
    internal static Task CallerArgumentExpressionAttribute_GuardsNulls()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsNullRefExceptionAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static Task CallerArgumentExpressionAttribute_NoParameterMutation()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsParameterMutationAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static void CallerArgumentExpressionAttribute_InternalOnly()
    {
        _TestType?.IsPublic.Assert().IsNot(true);
    }
}
