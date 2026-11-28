using System.Reflection;

namespace Werecodent.CreateAndFake.IssueReplication.Tests;

public sealed class IssueReplicationTests : AssemblyTester
{
    protected override Assembly CodeAssembly { get; } =
        Assembly.GetAssembly(typeof(IssueReplicationTests));

    public override void Tester_TestClassCoverage() { }
}
