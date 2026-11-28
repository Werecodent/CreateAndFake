using System.Reflection;

namespace Werecodent.CreateAndFake.Tests;

public sealed class ToolboxTests : AssemblyTester
{
    protected override Assembly CodeAssembly { get; } = Assembly.GetAssembly(typeof(ToolSet));

    [Fact]
    internal static async Task Toolbox_TestIndividual()
    {
        for (int i = 0; i < 100; i++)
        {
            await Tools.Tester.VerifyToolSetSupportAsync(
                [typeof(int?)],
                TestContext.Current.CancellationToken
            );
        }
    }
}
