using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RunnerTool.Attributes;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RunnerTool.Attributes;

public sealed class BaseStubAttributeTests : AutoTester<BaseStubAttribute>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                DisableToolSetSupportTests = true,
                IgnorableExceptions = [typeof(ToolException), typeof(ArgumentException)],
            };

    [Theory, RandomData]
    internal static void CreateParameterValue_CanSetFakesToBeStubs(
        [Stub] Fake<IIsGoodOrBadSample> sample
    )
    {
        sample.Dummy.GoodOrBadProp.Assert().Is(default(int));
    }
}
