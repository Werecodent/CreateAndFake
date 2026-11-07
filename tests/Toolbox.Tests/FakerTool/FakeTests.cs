using System.Reflection;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

public sealed class FakeTests : AutoTester<Fake>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { DisableParameterMutationTests = true };

    [Fact]
    internal static void Verify_NoTotalValid()
    {
        Tools.Faker.Mock<object>().Verify();
    }

    [Fact]
    internal static void Setup_WorksProtectedMethods()
    {
        MethodInfo method = typeof(ProtectedSample).GetMethod(
            "ChildMethod",
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        object[] args = [];

        Fake fake = Tools.Faker.Mock<ProtectedSample>();
        fake.Setup(method.Name, args, Behavior.None());
        fake.Verify(Times.Never, method.Name, args);

        method.Invoke(fake.Dummy, []);
        fake.Verify(Times.Once, method.Name, args);
    }
}
