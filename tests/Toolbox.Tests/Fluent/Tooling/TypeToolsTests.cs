using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Fluent.Tooling;

namespace Werecodent.CreateAndFake.Tests.Fluent.Tooling;

public sealed class TypeToolsTests : AutoTester<TypeTools>
{
    protected override TypeTools TestInstance { get; } = typeof(object).Tools();

    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                InjectionValues = [Tools.Gen.Next(5, 8)],
                IgnorableExceptions = [typeof(ToolException), typeof(InvalidCastException)],
            };

    public override Task Tester_ValueEqualityOverrideCheck()
    {
        return Task.CompletedTask;
    }
}
