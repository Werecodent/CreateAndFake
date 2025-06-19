using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="IDuplicator"/>
public interface IDuplicatorEngine
{
    /// <param name="chainer">Handles cloning child values.</param>
    /// <inheritdoc cref="IDuplicator.Copy{T}(T,DuplicatorMod)"/>
    [return: NotNullIfNotNull(nameof(source))]
    T Copy<T>(T source, IDuplicatorChainer chainer);
}
