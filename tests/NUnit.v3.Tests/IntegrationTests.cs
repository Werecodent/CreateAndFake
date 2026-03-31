using CreateAndFake.Design.Randomization;
using CreateAndFake.FakerTool;

namespace CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public static class IntegrationTests
{
    public sealed class Wrapper(IRandom gen)
    {
        public string NextName => gen.NextItem<string>([]);
    }

    [RandomData]
    public static void Integration_UsesParameterAttributes(
        [Fake] IRandom gen,
        Wrapper context,
        [Size(2)] string name
    )
    {
        name.Length.Assert().Is(2);
        gen.NextItem(Arg.Any<string[]>()).SetupReturn(name);
        context.NextName.Assert().Is(name);
    }

    [RandomData]
    public static void Issue118_FixesStubTypeRandomData([Stub] Type type)
    {
        type.Assert().IsNot(null);
    }
}
