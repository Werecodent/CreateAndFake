using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class TaskHelperTests
{
    [Fact]
    internal static Task TaskHelper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskHelper),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TaskHelper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskHelper),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task AwaitAsync_HandlesGenericTasks(string data)
    {
        return TaskHelper
            .AwaitAsync(Task.FromResult(data))
            .Assert()
            .HasResultAsync(data, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task AwaitAsync_HandlesTasks()
    {
        return TaskHelper
            .AwaitAsync(Task.CompletedTask)
            .Assert()
            .HasResultAsync(VoidType.Instance, TestContext.Current.CancellationToken);
    }
}
