using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public sealed class ExceptionCreateHandlersTests() : AutoTester(typeof(ExceptionCreateHandlers))
{
    [Fact]
    internal static void ExceptionCreateHandlers_InternalOnly()
    {
        typeof(ExceptionCreateHandlers).IsPublic.Assert().Is(false);
    }
}
