using System.Reflection;

namespace CreateAndFake.Design.Tests.Properties;

public static class RequiredMemberAttributeTests
{
    private static readonly Type _TestType = Assembly
        .GetAssembly(typeof(ArgumentGuard))
        .GetType("RequiredMemberAttribute");

    [Fact]
    internal static Task RequiredMemberAttribute_GuardsNulls()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsNullRefExceptionAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static Task RequiredMemberAttribute_NoParameterMutation()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsParameterMutationAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static void RequiredMemberAttribute_InternalOnly()
    {
        _TestType?.IsPublic.Assert().IsNot(true);
    }
}
