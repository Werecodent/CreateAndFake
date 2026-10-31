using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <summary>Checks if two objects are equal with async compatibility using the <paramref name="valuer"/>.</summary>
/// <typeparam name="T">Supported type for comparisons.</typeparam>
/// <param name="valuer"><inheritdoc cref="_valuer" path="/summary"/></param>
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
