using System.Reflection;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class AnyGenericTests
{
    [Fact]
    internal static void AnyGeneric_VoidTypePrivateConstructor()
    {
        ConstructorInfo constructor = typeof(AnyGeneric)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().IsNot(null);
    }
}
