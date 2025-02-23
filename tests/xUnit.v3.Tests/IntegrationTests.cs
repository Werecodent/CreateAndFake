using CreateAndFake.Design.Randomization;
using CreateAndFake.FakerTool;

namespace CreateAndFake.xUnit.v3.Tests;

public static class IntegrationTests
{
    public sealed class Wrapper(IRandom gen)
    {
        public string NextName => gen.NextItem<string>([]);
    }

    [Theory, RandomData]
    internal static void RandomData_UsesAttributes([Stub] IRandom gen, Wrapper context, [Size(2)] string name)
    {
        name.Length.Assert().Is(2);
        gen.NextItem(Arg.Any<string[]>()).SetupReturn(name);
        context.NextName.Assert().Is(name);
    }

    [Theory, RandomData]
    internal static void Issue118_FixesStubTypeRandomData([Stub] Type type)
    {
        type.Assert().IsNot(null);
    }
}
