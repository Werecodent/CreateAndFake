using Werecodent.CreateAndFake.Design.Properties;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tests.Properties;

public sealed class DesignDefaultsTests() : AutoTester(typeof(DesignDefaults))
{
    [Fact]
    internal static void IterationLimit_DoubleObjectSubclassMinimum()
    {
        (2 * TypeDescriber.For<object>().FindLoadedSubclasses().Count())
            .Assert()
            .LessThan(DesignDefaults.IterationLimit);
    }
}
