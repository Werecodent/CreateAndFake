using System.Reflection;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.Toolbox.FakerTool;

public static class AnyGenericTests
{
    [Fact]
    internal static void VoidType_PrivateConstructor()
    {
        ConstructorInfo constructor = typeof(AnyGeneric)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().IsNot(null);
    }
}
