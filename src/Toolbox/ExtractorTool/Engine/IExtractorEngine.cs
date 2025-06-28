using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool.Engine;

/// <summary>Extracts the contents of objects.</summary>
public interface IExtractorEngine : IToolEngine<ExtractHint>
{
    /// <summary>Temp</summary>
    /// <param name="value"></param>
    /// <param name="chainer"></param>
    /// <returns></returns>
    bool Extract(object? value, IExtractorChainer chainer);
}
