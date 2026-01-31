using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class ConfigurationSectionCreateHandlerTests
{
    [Fact]
    internal static void ConfigurationSectionCreateHandler_InternalOnly()
    {
        typeof(ConfigurationSectionCreateHandler).IsPublic.Assert().Is(false);
    }
}
