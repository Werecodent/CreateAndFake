using Werecodent.CreateAndFake.Design.Data;

namespace Werecodent.CreateAndFake.Design.Tests.Data;

public sealed class NameDataTests() : AutoTester(typeof(NameData))
{
    [Fact]
    internal static void Values_Populated()
    {
        NameData.Values.Assert().IsNotEmpty();
    }
}
