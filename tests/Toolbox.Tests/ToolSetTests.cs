namespace Werecodent.CreateAndFake.Tests;

public sealed class ToolSetTests : AutoTester<ToolSet>
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(FileNotFoundException)] };

    [Fact]
    internal static void ToolSet_Creatable()
    {
        ToolSet.CreateViaSeed(0).Assert().IsNotNull();
    }
}
