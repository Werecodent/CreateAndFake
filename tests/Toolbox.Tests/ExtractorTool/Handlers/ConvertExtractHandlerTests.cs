using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public static class ConvertExtractHandlerTests
{
    [Fact]
    internal static void ConvertExtractHandler_InternalOnly()
    {
        typeof(ConvertExtractHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void ExtractSupported_IgnoresDuplicates()
    {
        TypeDescriber describer = TypeDescriber.For(typeof(string));
        object[] content = [describer, describer];

        Tools.Extractor.Extract(content).FindAll<TypeDescriber>().Assert().HasCount(1);
    }

    [Fact]
    internal static async Task ExtractSupportedAsync_IgnoresDuplicates()
    {
        TypeDescriber describer = TypeDescriber.For(typeof(string));
        object[] content = [describer, describer];

        await (await Tools.Extractor.ExtractAsync(content, TestContext.Current.CancellationToken))
            .FindAllAsync<TypeDescriber>(TestContext.Current.CancellationToken)
            .Assert()
            .HasCountAsync(1, TestContext.Current.CancellationToken);
    }
}
