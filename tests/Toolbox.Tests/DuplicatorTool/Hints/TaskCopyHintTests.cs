using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class TaskCopyHintTests : CopyHintTestBase<TaskCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Task<DataHolderSample>),
        typeof(Task<object>),
        typeof(Task<string>),
    ];

    public TaskCopyHintTests()
        : base(_ValidTypes) { }

    [Fact]
    internal Task TryCopy_CopiesNonGenericTaskWhenCanceled()
    {
        Task task = Task.FromCanceled(new CancellationToken(true));
        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryCopy_CopiesNonGenericTaskWhenFaulted(Exception error)
    {
        Task task = Task.FromException(error);

        task.Exception.InnerException.Assert().Is(error);

        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal Task TryCopy_CopiesNonGenericTaskWhenCompleted()
    {
        Task task = Task.CompletedTask;
        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal Task TryCopy_CopiesNonGenericTaskWhenOngoing()
    {
        Task task = Task.Delay(1000, TestContext.Current.CancellationToken);
        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Fact]
    internal Task TryCopy_CopiesGenericTaskWhenCanceled()
    {
        Task task = Task.FromCanceled<string>(new CancellationToken(true));
        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryCopy_CopiesGenericTaskWhenFaulted(Exception error)
    {
        Task task = Task.FromException<string>(error);

        task.Exception.InnerException.Assert().Is(error);

        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryCopy_CopiesGenericTaskWhenCompleted(string data)
    {
        Task task = Task.FromResult(data);
        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryCopy_CopiesGenericTaskWhenOngoing(string data)
    {
        Task task = Task.Run(async () =>
        {
            await Task.Delay(1000, TestContext.Current.CancellationToken);
            return data;
        });
        return TestInstance
            .TryCopy(task, CreateChainer())
            .Data.Assert()
            .IsAsync(task, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task TryCopy_SourceCopiesWhenFaulted(Exception error)
    {
        TaskCompletionSource<object> source = new();
        source.SetException(error);

        return TestInstance
            .TryCopy(source, CreateChainer())
            .Data.Assert()
            .IsAsync(source, TestContext.Current.CancellationToken);
    }

#pragma warning disable MA0040, xUnit1051 // Not available in all .NET versions.
    [Fact]
    internal Task TryCopy_SourceCopiesWhenCanceled()
    {
        TaskCompletionSource<object> source = new();
        source.SetCanceled();

        return TestInstance
            .TryCopy(source, CreateChainer())
            .Data.Assert()
            .IsAsync(source, TestContext.Current.CancellationToken);
    }
#pragma warning restore MA0040, xUnit1051
}
