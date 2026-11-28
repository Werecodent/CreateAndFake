using System.Reflection;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public sealed class XUnitTests : AssemblyTester
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { AllowedTestNamePrefixes = ["Issue118"] };

    protected override Assembly CodeAssembly { get; } =
        Assembly.GetAssembly(typeof(RandomDataAttribute));
}
