using CreateAndFake.RandomizerTool;

namespace CreateAndFake.Tests.RandomizerTool;

public sealed class InfiniteLoopExceptionTests : ExceptionTestBase<InfiniteLoopException>
{
    [Theory, RandomData]
    internal static void InfiniteLoopException_HandlesNull(IEnumerable<Type> history)
    {
        _ = new InfiniteLoopException(null, history);
    }
}
