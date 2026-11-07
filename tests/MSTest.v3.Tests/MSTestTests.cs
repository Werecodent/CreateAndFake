using System.Reflection;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public sealed class MSTestTests : AssemblyTester
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { AllowedTestNamePrefixes = ["Issue118"] };

    protected override Assembly CodeAssembly { get; } =
        Assembly.GetAssembly(typeof(RandomDataAttribute));
}
