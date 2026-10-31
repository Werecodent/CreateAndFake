using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public sealed class FakeAttributeTests : AutoTester<FakeAttribute>
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt =>
            opt with
            {
                InjectionValues =
                [
                    IntegrationTests.AttributeMethod,
                    IntegrationTests
                        .AttributeMethod.GetParameters()
                        .First(p => Attribute.IsDefined(p, typeof(FakeAttribute))),
                ],
            };
}
