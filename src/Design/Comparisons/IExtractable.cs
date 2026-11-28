namespace Werecodent.CreateAndFake.Design.Comparisons;

/// <summary>Provides identifiable content extraction.</summary>
/// <remarks>The Extractor tool will use the implemented behavior.</remarks>
public interface IExtractable
{
    /// <summary>Finds all identifiable data contained by <see langword="this"/>.</summary>
    /// <returns>The identifiable data.</returns>
    IEnumerable<object?> ExtractContents();
}
