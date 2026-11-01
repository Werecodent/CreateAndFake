using NUnit.Framework;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public abstract class AutoTester(Type testType) : BaseAutoTester(testType)
{
    /// <inheritdoc/>
    [Test]
    public override Task Tester_GuardsNulls()
    {
        return GuardsNullsImplementation(CancellationTokenProvider.Get());
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_NoParameterMutation()
    {
        return NoParameterMutationImplementation(CancellationTokenProvider.Get());
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_ValueEqualityOverrideCheck()
    {
        return ValueEqualityOverrideCheckImplementation(CancellationTokenProvider.Get());
    }

    /// <inheritdoc/>
    [Test]
    public override void Tester_Debug_ToString()
    {
        DebugToStringImplementation();
    }
}
