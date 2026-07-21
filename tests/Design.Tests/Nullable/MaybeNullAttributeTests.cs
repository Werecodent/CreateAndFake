using System.Reflection;

namespace CreateAndFake.Design.Tests.Nullable;

public static class MaybeNullAttributeTests
{
    private static readonly Type _TestType = Assembly
        .GetAssembly(typeof(ArgumentGuard))
        .GetType("MaybeNullAttribute");

    [Fact]
    internal static Task MaybeNullAttribute_GuardsNulls()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsNullRefExceptionAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static Task MaybeNullAttribute_NoParameterMutation()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsParameterMutationAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static void MaybeNullAttribute_InternalOnly()
    {
        _TestType?.IsPublic.Assert().IsNot(true);
    }
}
