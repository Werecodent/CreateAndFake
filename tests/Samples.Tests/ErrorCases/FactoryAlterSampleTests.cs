using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public sealed class FactoryAlterSampleTests : AutoTester<FactoryAlterSample>
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { DisableToolSetSupportTests = true };
}
