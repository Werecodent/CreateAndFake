using System.Reflection;
using CreateAndFake.AsyncAsserterTool;

namespace CreateAndFake.Tests.AsyncAsserterTool;

public static class AsyncAsserterTests
{
    [Fact]
    internal static Task AsyncAsserter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AsyncAsserter>();
    }

    [Fact]
    internal static Task AsyncAsserter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AsyncAsserter>();
    }

    [Fact]
    internal static void AsyncAsserter_AllMethodsVirtual()
    {
        Tools.Asserter.IsEmpty(
            typeof(AsyncAsserter)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsVirtual)
                .Select(m => m.Name)
                .Where(n => n is not nameof(AsyncAsserter.Is) and not nameof(AsyncAsserter.IsNot))
                .Where(n => n is not $"get_{nameof(AsyncAsserter.Options)}")
        );
    }
}
