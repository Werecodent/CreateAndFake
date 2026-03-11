using System.Reflection;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class VoidReturnTests
{
    [Fact]
    internal static void VoidReturn_PrivateConstructor()
    {
        ConstructorInfo constructor = InheritanceTracker.For<VoidType>().Constructors.All.Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().Pass();
    }

    [Fact]
    internal static void Instance_Singleton()
    {
        VoidReturn.Instance.Assert().ReferenceEqual(VoidReturn.Instance);
    }
}
