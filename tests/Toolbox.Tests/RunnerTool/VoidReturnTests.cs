using System.Reflection;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class VoidReturnTests
{
    [Fact]
    internal static void VoidReturn_PrivateConstructor()
    {
        ConstructorInfo constructor = typeof(VoidType)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().Pass();
    }

    [Fact]
    internal static void Instance_Singleton()
    {
        VoidReturn.Instance.Assert().ReferenceEqual(VoidReturn.Instance);
    }
}
