using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public sealed class ValueCreateHandlersTests() : AutoTester(typeof(ValueCreateHandlers))
{
    [Fact]
    internal static void ValueCreateHandlers_InternalOnly()
    {
        typeof(ValueCreateHandlers).IsPublic.Assert().Is(false);
    }
}
