using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public sealed class SizeAttributeTests : AutoTester<SizeAttribute>
{
    protected override SizeAttribute TestInstance =>
        new(Tools.Gen.Next(1, 3), Tools.Gen.Next(3, 5));

    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
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
