using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.MSTest.v4;

/// <inheritdoc/>
public abstract class AutoTester<T> : BaseAutoTester<T>
{
    /// <summary>Used to store information that is provided to unit tests for the MSTest framework.</summary>
    public TestContext? TestContext { get; set; }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_GuardsNulls()
    {
        return GuardsNullsImplementation(TestContext!.CancellationToken);
    }

    /// <inheritdoc/>
    [TestMethod]
    public override Task Tester_NoParameterMutation()
    {
        return NoParameterMutationImplementation(TestContext!.CancellationToken);
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
