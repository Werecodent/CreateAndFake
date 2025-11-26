using CreateAndFake.Design.Randomization;
using CreateAndFake.FakerTool;

namespace CreateAndFake.MSTest.v4.Tests;

[TestClass]
public class IntegrationTests
{
    public sealed class Wrapper(IRandom gen)
    {
        public string NextName => gen.NextItem<string>([]);
    }

    [TestMethod, RandomData]
    public void RandomData_UsesAttributes(
        [Stub] IRandom gen,
        Wrapper context,
        [Size(2)] string name
    )
    {
        name.Length.Assert().Is(2);
        gen.NextItem(Arg.Any<string[]>()).SetupReturn(name);
        context.NextName.Assert().Is(name);
    }

    [TestMethod, RandomData]
    public void Issue118_FixesStubTypeRandomData([Stub] Type type)
    {
        type.Assert().IsNot(null);
    }
}
