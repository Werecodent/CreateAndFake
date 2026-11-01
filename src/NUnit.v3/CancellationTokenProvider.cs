using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <summary>Accesses the framework provides <see cref="CancellationToken"/> for tests.</summary>
[ExcludeFromCodeCoverage] // Missing case cannot be covered by updated test frameworks.
internal static class CancellationTokenProvider
{
    /// <summary>Property used to access the current test's <see cref="CancellationToken"/>.</summary>
    /// <remarks>NUnit v4.1.0 required. Cannot use as a dependency due to not supporting .NET Standard 2.0.</remarks>
    private static readonly Func<TestContext, CancellationToken>? _CancellationTokenGrabber = (Func<
        TestContext,
        CancellationToken
    >?)
        typeof(TestContext)
            .GetProperty("CancellationToken")
            ?.GetGetMethod()!
            .CreateDelegate(typeof(Func<TestContext, CancellationToken>));

    /// <summary>Retrieves the current test's <see cref="CancellationToken"/>.</summary>
    /// <returns>Valid token if available, default token if not.</returns>
    internal static CancellationToken Get()
    {
        return _CancellationTokenGrabber?.Invoke(TestContext.CurrentContext)
            ?? CancellationToken.None;
    }
}
