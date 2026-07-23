using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class MaybeAsyncCancelerTests
{
    [Fact]
    internal static Task MaybeAsyncCanceler_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            MaybeAsyncCanceler.Use,
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentException)] }
        );
    }

    [Fact]
    internal static Task MaybeAsyncCanceler_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            MaybeAsyncCanceler.Use,
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentException)] }
        );
    }

    [Fact]
    internal static void MaybeAsyncCanceler_InternalOnly()
    {
        typeof(MaybeAsyncCanceler).IsPublic.Assert().IsNot(true);
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_Cancels()
    {
        using CancellationTokenSource source = new();
        await MaybeAsyncCanceler.Use.TriggerCancellationAsync(source);
        source.IsCancellationRequested.Assert().Is(true);
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_MultipleCancelSafe()
    {
        using CancellationTokenSource source = new();
        await MaybeAsyncCanceler.Use.TriggerCancellationAsync(source);
        await MaybeAsyncCanceler.Use.TriggerCancellationAsync(source);
    }

    [Fact]
    internal static async Task TriggerCancellationAsync_SyncFallbackCancels()
    {
        using CancellationTokenSource source = new();
        await new MaybeAsyncCanceler(null).TriggerCancellationAsync(source);
        source.IsCancellationRequested.Assert().Is(true);
    }
}
