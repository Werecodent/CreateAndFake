using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public sealed class ReflectionCompareHandlersTests() : AutoTester(typeof(ReflectionCompareHandlers))
{
    [Fact]
    internal static void ReflectionCompareHandlers_InternalOnly()
    {
        typeof(ReflectionCompareHandlers).IsPublic.Assert().Is(false);
    }
}
