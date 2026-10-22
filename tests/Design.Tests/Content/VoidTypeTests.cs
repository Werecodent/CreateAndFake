using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class VoidTypeTests
{
    [Fact]
    internal static Task VoidType_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<VoidType>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task VoidType_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<VoidType>(
            TestContext.Current.CancellationToken
        );
    }

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
