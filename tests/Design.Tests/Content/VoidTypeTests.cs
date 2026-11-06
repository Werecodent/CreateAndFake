using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class VoidTypeTests : AutoTester<VoidType>
{
    [Fact]
    internal static void VoidType_PrivateConstructor()
    {
        ConstructorInfo constructor = TypeDescriber.For<VoidType>().Constructors.All.Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().Pass();
    }

    [Fact]
    internal static void Instance_Singleton()
    {
        VoidType.Instance.Assert().ReferenceEqual(VoidType.Instance);
    }
}
