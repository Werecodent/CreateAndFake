using Microsoft.VisualStudio.TestTools.UnitTesting;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.MSTest.v3;

/// <inheritdoc/>
[TestClass]
public abstract class AutoTester(Type testType) : BaseAutoTester(testType)
{
    /// <summary>Used to store information that is provided to unit tests for the MSTest framework.</summary>
    public TestContext? TestContext { get; set; }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_GuardsNulls()
    {
        return GuardsNullsImplementation(TestContext!.CancellationTokenSource.Token);
    }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_NoParameterMutation()
    {
        return NoParameterMutationImplementation(TestContext!.CancellationTokenSource.Token);
    }

    /// <inheritdoc/>
    [TestMethod]
    public override void Tester_ValueEqualityOverrideCheck()
    {
        ValueEqualityOverrideCheckImplementation();
    }

    /// <inheritdoc/>
    [TestMethod]
    public override void Tester_Debug_ToString()
    {
        DebugToStringImplementation();
    }
}
