using CreateAndFake.Design.Randomization;
using CreateAndFake.FakerTool;

namespace CreateAndFake.xUnit.v2.Tests;

public static class IntegrationTests
{
    public sealed class Wrapper(IRandom gen)
    {
        public string NextName => gen.NextItem<string>([]);
    }

    [Theory, RandomData]
    internal static void Integration_UsesParameterAttributes(
        [Stub] IRandom gen,
        Wrapper context,
        [Size(2)] string name
    )
    {
        name.Length.Assert().Is(2);
        gen.NextItem(Arg.Any<string[]>()).SetupReturn(name);
        context.NextName.Assert().Is(name);
    }

#if !LEGACY // xUnit needs Type parameters to inherit IReflectableType, which cannot be faked in legacy .NET.
    [Theory, RandomData]
    internal static void Issue118_FixesStubTypeRandomData([Stub] Type type)
    {
        type.Assert().IsNotNull();
    }
#endif
}
