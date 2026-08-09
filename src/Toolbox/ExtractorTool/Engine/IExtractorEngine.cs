using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Extracts the contents of objects.</summary>
public interface IExtractorEngine : IToolEngine<IExtractHint>
{
    /// <summary>Temp</summary>
    /// <param name="value"></param>
    /// <param name="chainer"></param>
    /// <returns></returns>
    bool Extract(object? value, IExtractorChainer chainer);
}
