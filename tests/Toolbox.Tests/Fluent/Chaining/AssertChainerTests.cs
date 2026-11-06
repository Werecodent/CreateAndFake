using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

public sealed class AssertChainerTests : AutoTester<AssertChainer<StreamWriter>>
{
    [Theory, RandomData]
    internal static void And_ReturnsInput(object data)
    {
        new AssertChainer<object>(data, Tools.Asserter).And().Assert().Is(data);
    }
}
