using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public sealed class ConfigurationSectionCreateHandlerTests()
    : AutoTester(typeof(ConfigurationSectionCreateHandler))
{
    [Fact]
    internal static void ConfigurationSectionCreateHandler_InternalOnly()
    {
        typeof(ConfigurationSectionCreateHandler).IsPublic.Assert().Is(false);
    }
}
