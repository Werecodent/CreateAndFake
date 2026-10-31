using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.MSTest.v4.Tests;

[TestClass]
public sealed class SizeAttributeTests : AutoTester<SizeAttribute>
{
    protected override SizeAttribute TestInstance { get; } =
        new SizeAttribute(Tools.Gen.Next(1, 3), Tools.Gen.Next(3, 5));

    protected override Func<TesterOptions, TesterOptions> OptionConfiguration =>
        opt =>
            opt with
            {
                InjectionValues =
                [
                    IntegrationTests.AttributeMethod,
                    IntegrationTests
                        .AttributeMethod.GetParameters()
                        .First(p => Attribute.IsDefined(p, typeof(SizeAttribute))),
                ],
            };
}
