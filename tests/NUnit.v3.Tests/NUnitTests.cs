using System.Reflection;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public sealed class NUnitTests : AssemblyTester
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { AllowedTestNamePrefixes = ["Issue118"] };

    protected override Assembly CodeAssembly { get; } =
        Assembly.GetAssembly(typeof(RandomDataAttribute));
}
