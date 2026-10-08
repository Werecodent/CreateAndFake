using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class ValueTaskCopyHintTests : CopyHintTestBase<ValueTaskCopyHint>
{
    [Fact]
    internal async Task TryCopy_PreventsCloningExternalValueTasks()
    {
        ValueTask task = new(Task.CompletedTask);
        try
        {
            TestInstance
                .Assert(x => x.TryCopy(task, CreateChainer()))
                .Throws<UnsupportedException>();
        }
        finally
        {
            await task;
        }
    }

    [Theory, RandomData]
    internal async Task TryCopy_PreventsCloningExternalGenericValueTasks(string data)
    {
        ValueTask<string> task = new(Task.FromResult(data));
        try
        {
            TestInstance
                .Assert(x => x.TryCopy(task, CreateChainer()))
                .Throws<UnsupportedException>();
        }
        finally
        {
            await task;
        }
    }
}
