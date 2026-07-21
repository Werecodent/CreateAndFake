using System.Reflection;

namespace CreateAndFake.Design.Tests.Properties;

public static class SetsRequiredMembersAttributeTests
{
    private static readonly Type _TestType = Assembly
        .GetAssembly(typeof(ArgumentGuard))
        .GetType("SetsRequiredMembersAttribute");

    [Fact]
    internal static Task SetsRequiredMembersAttribute_GuardsNulls()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsNullRefExceptionAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static Task SetsRequiredMembersAttribute_NoParameterMutation()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsParameterMutationAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static void SetsRequiredMembersAttribute_InternalOnly()
    {
        _TestType?.IsPublic.Assert().IsNot(true);
    }
}
