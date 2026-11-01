using System.Reflection;
using Werecodent.CreateAndFake.FakerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.FakerTool.Engine;

public sealed class FakerChainerTests : AutoTester<FakerChainer>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions =
                [
                    typeof(ArgumentException),
                    typeof(InvalidOperationException),
                    typeof(ArgumentOutOfRangeException),
                    typeof(TargetInvocationException),
                ],
            };
}
