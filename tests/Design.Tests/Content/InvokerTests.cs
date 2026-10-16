using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class InvokerTests
{
    [Fact]
    internal static Task Invoker_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Invoker),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task Invoker_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Invoker),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task AwaitAsync_HandlesGenericTasks(string data)
    {
        return Invoker
            .AwaitAsync(Task.FromResult(data))
            .Assert()
            .HasResultAsync(data, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task AwaitAsync_HandlesTasks()
    {
        return Invoker
            .AwaitAsync(Task.CompletedTask)
            .Assert()
            .HasResultAsync(VoidType.Instance, TestContext.Current.CancellationToken);
    }
}
