using Werecodent.CreateAndFake.TesterTool;
using Xunit;

namespace Werecodent.CreateAndFake.xUnit.v3;

/// <inheritdoc/>
public abstract class AutoTester(Type testType) : BaseAutoTester(testType)
{
    /// <inheritdoc/>
    [Fact]
    public override Task Tester_GuardsNulls()
    {
        return GuardsNullsImplementation(TestContext.Current.CancellationToken);
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_NoParameterMutation()
    {
        return NoParameterMutationImplementation(TestContext.Current.CancellationToken);
    }

    /// <inheritdoc/>
    [Fact]
    public override void Tester_ValueEqualityOverrideCheck()
    {
        ValueEqualityOverrideCheckImplementation();
    }

    /// <inheritdoc/>
    [Fact]
    public override void Tester_Debug_ToString()
    {
        DebugToStringImplementation();
    }
}
