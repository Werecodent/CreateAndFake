using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Handlers;

/// <summary>
///     Handles <typeparamref name="T"/> cloning via the <paramref name="factory"/>.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">Behavior handling <typeparamref name="T"/> cloning.</param>
internal sealed class FactoryCopyHandler<T>(Func<T, IDuplicatorChainer, T> factory) : ICopyHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public object? CopySupported(object source, IDuplicatorChainer duplicator)
    {
        return factory.Invoke((T)source, duplicator);
    }
}
