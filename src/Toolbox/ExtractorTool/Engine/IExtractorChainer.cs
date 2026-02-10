using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool.Engine;

/// <summary>Provides a callback into <see cref="IExtractor"/> to extract child values.</summary>
public interface IExtractorChainer : IExtractor, IToolChainer<ExtractorOptions, IExtractHint>
{
    /// <summary>Temp</summary>
    /// <param name="value"></param>
    /// <param name="optionConfiguration"></param>
    /// <returns></returns>
    bool AddFoundValue(object value, ExtractorMod? optionConfiguration = null);

    /// <summary>Temp</summary>
    /// <param name="value"></param>
    /// <param name="optionConfiguration"></param>
    /// <returns></returns>
    bool InnerExtract(object? value, ExtractorMod? optionConfiguration = null);
}
