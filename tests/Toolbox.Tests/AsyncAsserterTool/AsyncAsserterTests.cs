using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.Tests.AsyncAsserterTool;

public static class AsyncAsserterTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IgnorableExceptions =
            [
                typeof(AssertException),
                typeof(ArgumentException),
                typeof(NotSupportedException),
                typeof(InsufficientExecutionStackException),
                typeof(ToolException),
                typeof(TargetException),
            ],
        };

    [Fact]
    internal static Task AsyncAsserter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AsyncAsserter>(config);
    }

    [Fact]
    internal static Task AsyncAsserter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AsyncAsserter>(config);
    }

    [Fact]
    internal static void AsyncAsserter_AllMethodsVirtual()
    {
        Tools.Asserter.IsEmpty(
            typeof(AsyncAsserter)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsVirtual)
                .Select(m => m.Name)
                .Where(n => n is not nameof(AsyncAsserter.IsAsync))
                .Where(n => n is not nameof(AsyncAsserter.IsNotAsync))
                .Where(n => n is not $"get_{nameof(AsyncAsserter.Options)}")
        );
    }
}
