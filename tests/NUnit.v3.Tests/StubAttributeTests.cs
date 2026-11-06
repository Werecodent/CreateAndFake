using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public sealed class StubAttributeTests : AutoTester<StubAttribute>
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
                        .First(p => Attribute.IsDefined(p, typeof(StubAttribute))),
                ],
            };
}
