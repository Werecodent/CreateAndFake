using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.TesterTool.Validators;

#pragma warning disable CS8425 // Configured as given for testing.

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class CancelValidator(TesterOptions options)
{
    /// <inheritdoc cref="Tester.Options"/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    public Task VerifySupportsCancellationAsync<T>(
        Func<int, IAsyncEnumerable<T>> factory,
        CancellationToken canceler
    )
    {
        return VerifyUsesWithCancellationAsync((size, _) => factory(size), canceler);
    }

    public async Task VerifySupportsCancellationAsync<T>(
        Func<int, CancellationToken, IAsyncEnumerable<T>> factory,
        CancellationToken canceler
    )
    {
        await VerifyUsesGivenCancellationTokenAsync(factory, canceler).ConfigureAwait(false);
        await VerifyUsesWithCancellationAsync(factory, canceler).ConfigureAwait(false);
    }

    private Task VerifyUsesWithCancellationAsync<T>(
        Func<int, CancellationToken, IAsyncEnumerable<T>> factory,
        CancellationToken canceler
    )
    {
        async IAsyncEnumerable<T> iterateUsingForeachAsync(
            int size,
            int cancelPoint,
            CancellationToken canceler
        )
        {
            using CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(
                canceler
            );

            if (cancelPoint == 0)
            {
                await AsyncSeriesHelper.TriggerCancellationAsync(source).ConfigureAwait(false);
            }

            int i = 0;
            await foreach (
                T item in factory
                    .Invoke(size, canceler)
                    .WithCancellation(source.Token)
                    .ConfigureAwait(false)
            )
            {
                yield return item;

                if (++i == cancelPoint)
                {
                    await AsyncSeriesHelper.TriggerCancellationAsync(source).ConfigureAwait(false);
                    await Task.Delay(50, canceler).ConfigureAwait(false);
                }
            }
        }

        return RunCancelTestsAsync(iterateUsingForeachAsync, "using WithCancellation", canceler);
    }

    private Task VerifyUsesGivenCancellationTokenAsync<T>(
        Func<int, CancellationToken, IAsyncEnumerable<T>> factory,
        CancellationToken canceler
    )
    {
        async IAsyncEnumerable<T> iterateUsingForeachAsync(
            int size,
            int cancelPoint,
            CancellationToken canceler
        )
        {
            using CancellationTokenSource source = CancellationTokenSource.CreateLinkedTokenSource(
                canceler
            );

            if (cancelPoint == 0)
            {
                await AsyncSeriesHelper.TriggerCancellationAsync(source).ConfigureAwait(false);
            }

            int i = 0;
            await foreach (T item in factory.Invoke(size, source.Token).ConfigureAwait(false))
            {
                yield return item;

                if (++i == cancelPoint)
                {
                    await AsyncSeriesHelper.TriggerCancellationAsync(source).ConfigureAwait(false);
                }
            }
        }

        return RunCancelTestsAsync(iterateUsingForeachAsync, "given a token directly", canceler);
    }

    private async Task RunCancelTestsAsync<T>(
        Func<int, int, CancellationToken, IAsyncEnumerable<T>> modifiedTestFactory,
        string testNote,
        CancellationToken canceler
    )
    {
        Task runThrowTestAsync(int size, int cancelPoint)
        {
            return options.Asserter.ThrowsAsync<OperationCanceledException, T>(
                modifiedTestFactory(size, cancelPoint, canceler),
                canceler,
                $"ThrowIfCancellationRequested() missing when {testNote}: Size={size}, Cancellation Point={cancelPoint}"
            );
        }

        Task runPassTestAsync(int size, int cancelPoint)
        {
            return options.Asserter.HasCountAsync(
                size,
                modifiedTestFactory(size, cancelPoint, canceler),
                canceler,
                $"Results should be returned fully when {testNote}: Size={size}, Cancellation Point={cancelPoint}"
            );
        }

        if (Options.ThrowsUponCancelWithEmptySeries)
        {
            await runThrowTestAsync(0, 0).ConfigureAwait(false);
        }
        else
        {
            await runPassTestAsync(0, 0).ConfigureAwait(false);
        }

        await runThrowTestAsync(1, 0).ConfigureAwait(false);
        await runThrowTestAsync(2, 1).ConfigureAwait(false);

        await runPassTestAsync(1, 1).ConfigureAwait(false);
        await runPassTestAsync(2, 2).ConfigureAwait(false);
    }
}

#pragma warning restore
