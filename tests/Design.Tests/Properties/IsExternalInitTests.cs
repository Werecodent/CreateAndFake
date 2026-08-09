using System.Reflection;

namespace Werecodent.CreateAndFake.Design.Tests.Properties;

public static class IsExternalInitTests
{
    private static readonly Type _TestType = Assembly
        .GetAssembly(typeof(ArgumentGuard))
        .GetType("IsExternalInit");

    [Fact]
    internal static Task IsExternalInit_GuardsNulls()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsNullRefExceptionAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static Task IsExternalInit_NoParameterMutation()
    {
        return _TestType == null
            ? Task.CompletedTask
            : Tools.Tester.PreventsParameterMutationAsync(
                _TestType,
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    internal static void IsExternalInit_InternalOnly()
    {
        _TestType?.IsPublic.Assert().IsNot(true);
    }
}
