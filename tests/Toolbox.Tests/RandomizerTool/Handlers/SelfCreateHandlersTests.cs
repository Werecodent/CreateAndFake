using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public sealed class SelfCreateHandlersTests() : AutoTester(typeof(SelfCreateHandlers))
{
    [Fact]
    internal static void SelfCreateHandlers_InternalOnly()
    {
        typeof(SelfCreateHandlers).IsPublic.Assert().Is(false);
    }
}
