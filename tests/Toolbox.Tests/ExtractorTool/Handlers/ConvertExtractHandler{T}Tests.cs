using Werecodent.CreateAndFake.ExtractorTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Handlers;

public sealed class ConvertExtractHandler_T_Tests() : AutoTester(typeof(ConvertExtractHandler<>))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(InvalidCastException)] };
}
