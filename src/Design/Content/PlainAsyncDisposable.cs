namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Simple implementation for <see cref="IAsyncDisposable"/>.</summary>
/// <param name="data">Result for <see cref="GrabData"/> when the instance has not been disposed.</param>
/// <remarks>Important for .NET 4.8 to have a creatable <see cref="IAsyncDisposable"/> for the framework.</remarks>
public sealed class PlainAsyncDisposable(string data) : IAsyncDisposable
{
    /// <summary>If <see cref="DisposeAsync"/> has been called.</summary>
    private bool _isDisposed = false;

    /// <summary>Access the held data if this has not been disposed.</summary>
    /// <returns>The held data.</returns>
    /// <exception cref="ObjectDisposedException">If <see cref="DisposeAsync"/> was called before this.</exception>
    public string GrabData()
    {
        return !_isDisposed
            ? data
            : throw new ObjectDisposedException("Instance was disposed before accessing its data.");
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        _isDisposed = true;
        return new ValueTask(Task.CompletedTask);
    }
}
