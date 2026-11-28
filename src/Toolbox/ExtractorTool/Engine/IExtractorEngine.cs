using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Extracts the contents of objects.</summary>
public interface IExtractorEngine : IToolEngine<IExtractHint>
{
    /// <inheritdoc cref="ExtractAsync"/>
    bool Extract(object? value, IExtractorChainer chainer);

    /// <summary>Attempts to extract data from the <paramref name="value"/>.</summary>
    /// <param name="value">Instance to extract data from.</param>
    /// <param name="chainer">Handles extracting child values.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns><see langword="true"/> if data was extracted, <see langword="false"/> otherwise.</returns>
    Task<bool> ExtractAsync(object? value, IExtractorChainer chainer, CancellationToken canceler);
}
