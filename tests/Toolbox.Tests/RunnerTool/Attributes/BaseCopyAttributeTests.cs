using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.RunnerTool.Attributes;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public sealed class BaseCopyAttributeTests : AutoTester<BaseCopyAttribute>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ToolException)] };

    [Fact]
    internal static Task CreateParameterValue_PreventsNonValue()
    {
        return Tools
            .Runner.CreateForAsync(
                typeof(BaseCopyAttributeTests).GetMethod(
                    nameof(BadNothingToCopy),
                    BindingFlags.Static | BindingFlags.NonPublic
                ),
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    private static DataSample BadNothingToCopy([Copy] DataSample sample)
    {
        return sample;
    }
}
