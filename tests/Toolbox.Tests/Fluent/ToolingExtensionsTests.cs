using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.Tests.Fluent;

public sealed class ToolingExtensionsTests() : AutoTester(typeof(ToolingExtensions))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(ToolException)] };
}
