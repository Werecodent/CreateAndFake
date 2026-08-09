using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="IDuplicator"/>
public interface IDuplicatorEngine : IToolEngine<CopyHint>
{
    /// <param name="chainer">Handles cloning child values.</param>
    /// <inheritdoc cref="IDuplicator.Copy{T}(T,DuplicatorMod)"/>
    [return: NotNullIfNotNull(nameof(source))]
    T Copy<T>(T source, IDuplicatorChainer chainer);
}
