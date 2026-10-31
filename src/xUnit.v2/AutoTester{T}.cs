using Werecodent.CreateAndFake.TesterTool;
using Xunit;

namespace Werecodent.CreateAndFake.xUnit.v2;

/// <inheritdoc/>
public abstract class AutoTester<T> : BaseAutoTester<T>
{
    /// <inheritdoc/>
    [Fact]
    public override Task Tester_GuardsNulls()
    {
        return GuardsNullsImplementation(CancellationToken.None);
    }

    /// <inheritdoc/>
    [Fact]
    public override Task Tester_NoParameterMutation()
    {
        return NoParameterMutationImplementation(CancellationToken.None);
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
