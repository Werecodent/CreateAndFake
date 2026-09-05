using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.RunnerTool;

public static class VoidReturnTests
{
    [Fact]
    internal static void VoidReturn_PrivateConstructor()
    {
        ConstructorInfo constructor = TypeDescriber.For<VoidType>().Constructors.All.Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().Pass();
    }

    [Fact]
    internal static void Instance_Singleton()
    {
        VoidReturn.Instance.Assert().ReferenceEqual(VoidReturn.Instance);
    }
}
