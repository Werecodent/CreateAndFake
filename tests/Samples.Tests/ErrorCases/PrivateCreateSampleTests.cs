using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public static class PrivateCreateSampleTests
{
    [Fact]
    internal static void PrivateCreateSample_PrivateConstructor()
    {
        ConstructorInfo constructor = TypeDescriber
            .For<PrivateCreateSample>()
            .Constructors.All.Single();

        constructor.IsPrivate.Assert().Is(true);
        constructor.Invoke([]).Assert().Pass();
    }
}
