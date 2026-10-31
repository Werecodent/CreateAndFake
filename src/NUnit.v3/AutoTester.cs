using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.NUnit.v3;

#pragma warning disable CA1822, S2325 // Property set for the instance.

/// <inheritdoc/>
public abstract class AutoTester(Type testType) : BaseAutoTester(testType)
{
    /// <summary>Property used to access the current test's <see cref="CancellationToken"/>.</summary>
    /// <remarks>NUnit v4.1.0 required. Cannot use as a dependency due to not supporting .NET Standard 2.0.</remarks>
    [ExcludeFromCodeCoverage] // Missing case cannot be covered by updated test frameworks.
    private static Func<TestContext, CancellationToken>? CancellationTokenGrabber { get; } =
        (Func<TestContext, CancellationToken>?)
            typeof(TestContext)
                .GetProperty("CancellationToken")
                ?.GetGetMethod()!
                .CreateDelegate(typeof(Func<TestContext, CancellationToken>));

    /// <summary>Retrieves the current test's <see cref="CancellationToken"/>.</summary>
    /// <returns>Valid token if available, default token if not.</returns>
    [ExcludeFromCodeCoverage] // Null case cannot be covered by updated test frameworks.
    internal static CancellationToken GetCancellationToken()
    {
        return CancellationTokenGrabber?.Invoke(TestContext.CurrentContext)
            ?? CancellationToken.None;
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_GuardsNulls()
    {
        return GuardsNullsImplementation(GetCancellationToken());
    }

    /// <inheritdoc/>
    [Test]
    public override Task Tester_NoParameterMutation()
    {
        return NoParameterMutationImplementation(GetCancellationToken());
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
