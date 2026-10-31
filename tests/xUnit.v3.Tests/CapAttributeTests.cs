using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

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
}
