using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public sealed class InjectAttributeTests : AutoTester<InjectAttribute>
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
                        .First(p => Attribute.IsDefined(p, typeof(InjectAttribute))),
                ],
            };
}
