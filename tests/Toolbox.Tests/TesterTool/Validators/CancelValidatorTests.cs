using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.TesterTool.Validators;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Validators;

#pragma warning disable MA0079 // For testing.

public static class CancelValidatorTests
{
    [Fact]
    internal static Task CancelValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CancelValidator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task CancelValidator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CancelValidator>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task VerifySupportsCancellationAsync_CanIgnoreCancelWithEmptySeries()
    {
        static async IAsyncEnumerable<string> iterate(
            IAsyncEnumerable<string> series,
            [EnumeratorCancellation] CancellationToken canceler = default
        )
        {
            await foreach (string item in series)
            {
                canceler.ThrowIfCancellationRequested();
                yield return item;
            }
        }

        return Tools
            .Tester.VerifySupportsCancellationAsync(
                (size, canceler) =>
                    iterate(Tools.Randomizer.CreateSized<IAsyncEnumerable<string>>(size), canceler),
                TestContext.Current.CancellationToken,
                opt => opt with { ThrowsUponCancelWithEmptySeries = false }
            )
            .Assert()
            .ThrowsNoExceptionAsync(TestContext.Current.CancellationToken);
    }
}

#pragma warning restore
