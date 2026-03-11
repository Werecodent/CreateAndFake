using System.Reflection;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class VoidTypeTests
{
    [Fact]
    internal static void VoidType_PrivateConstructor()
    {
        ConstructorInfo constructor = InheritanceTracker.For<VoidType>().Constructors.All.Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().Pass();
    }
}
