using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractorChainerTests
{
    [Fact]
    internal static Task ExtractorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<IExtractorChainer>();
    }

    [Fact]
    internal static Task ExtractorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<IExtractorChainer>();
    }
}
