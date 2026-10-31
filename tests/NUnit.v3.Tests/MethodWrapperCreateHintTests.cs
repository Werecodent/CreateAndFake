using NUnit.Framework.Internal;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public sealed class MethodWrapperCreateHintTests : AutoTester<MethodWrapperCreateHint>
{
    [Test]
    public static void Create_AddedSupport()
    {
        Tools.Randomizer.Create<MethodWrapper>().Assert().IsNotNull();
    }
}
