using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public sealed class CapAttributeTests : AutoTester<CapAttribute>
{
    protected override CapAttribute TestInstance { get; } = new(5, 10);

    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions = [typeof(ArgumentOutOfRangeException)],
                InjectionValues =
                [
                    IntegrationTests.AttributeMethod,
                    IntegrationTests
                        .AttributeMethod.GetParameters()
                        .First(p => Attribute.IsDefined(p, typeof(CapAttribute))),
                ],
            };

    [TestMethod]
    public void Clone_BranchesOnMinSet()
    {
        new CapAttribute(1).Clone().Assert().Is(new CapAttribute(1));
        new CapAttribute(1, 2).Clone().Assert().Is(new CapAttribute(1, 2));
    }
}
