using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <typeparam name="T">Supported <see cref="Type"/> for comparisons.</typeparam>
/// <param name="valuer"><inheritdoc cref="_valuer" path="/summary"/></param>
/// <inheritdoc cref="IAsyncEqualityComparer{T}"/>
public sealed class ByValuerAsyncComparer<T>(IValuer valuer) : IAsyncEqualityComparer<T>
{
    /// <inheritdoc cref="IValuer" path="/summary"/>
    private readonly IValuer _valuer = valuer ?? throw new ArgumentNullException(nameof(valuer));

    /// <inheritdoc/>
    public Task<bool> EqualsAsync(T? x, T? y, CancellationToken canceler)
    {
        return _valuer.EqualsAsync(x, y, canceler);
    }

    /// <inheritdoc/>
    public Task<int> GetHashCodeAsync(T? obj, CancellationToken canceler)
    {
        return _valuer.GetHashCodeAsync(obj, canceler);
    }
}
