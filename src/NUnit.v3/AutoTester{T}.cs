using NUnit.Framework;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.NUnit.v3;

#pragma warning disable CA1822, S2325 // Property set for the instance.

/// <inheritdoc/>
public abstract class AutoTester<T> : BaseAutoTester<T>
{
    /// <inheritdoc/>
    [Test]
    public override Task Tester_GuardsNulls()
    {
        return GuardsNullsImplementation(AutoTester.GetCancellationToken());
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_NoParameterMutation()
    {
        return NoParameterMutationImplementation(AutoTester.GetCancellationToken());
    }

    /// <inheritdoc/>
    [Test]
    public override void Tester_ValueEqualityOverrideCheck()
    {
        ValueEqualityOverrideCheckImplementation();
    }

    /// <inheritdoc/>
    [Test]
    public override void Tester_Debug_ToString()
    {
        DebugToStringImplementation();
    }
}

#pragma warning restore
