namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class SelfReferencingDisposable(string data) : IDisposable, IAsyncDisposable
{
    private bool _isDisposed = false;

    public string GrabData()
    {
        return !_isDisposed
            ? data
            : throw new ObjectDisposedException("Instance was disposed before accessing its data.");
    }

    public SelfReferencingDisposable GetSelf()
    {
        return !_isDisposed
            ? this
            : throw new ObjectDisposedException("Instance was disposed before accessing itself.");
    }

    public void Dispose()
    {
        _isDisposed = true;
    }

    public ValueTask DisposeAsync()
    {
        _isDisposed = true;
        return new ValueTask(Task.CompletedTask);
    }
}
