using System.Reflection;

namespace Werecodent.CreateAndFake.Design.Tests;

public sealed class DesignTests : AssemblyTester
{
    protected override Assembly CodeAssembly { get; } = Assembly.GetAssembly(typeof(ArgumentGuard));
}
