using System.Reflection;

namespace CreateAndFake.Design.Tests.Nullable;

public static class NotNullAttributeTests
{
    private static readonly Type _TestType = Assembly
        .GetAssembly(typeof(ArgumentGuard))
        .GetType("NotNullAttribute");

    [Fact]
    internal static Task NotNullAttribute_GuardsNulls()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsNullRefExceptionAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static Task NotNullAttribute_NoParameterMutation()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsParameterMutationAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static void NotNullAttribute_InternalOnly()
    {
        _TestType?.IsPublic.Assert().IsNot(true);
    }
}
