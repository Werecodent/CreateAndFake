using Werecodent.CreateAndFake.ValuerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Handlers;

public sealed class ConvertCompareHandlerTests() : AutoTester(typeof(ConvertCompareHandler<>))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(InvalidCastException)] };

    [Fact]
    internal static void ConvertCompareHandler_InternalOnly()
    {
        typeof(ConvertCompareHandler<>).IsPublic.Assert().Is(false);
    }
}
