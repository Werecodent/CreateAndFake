using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample, ExcludeFromCreateAndFake]
public sealed class FactoryAlterSample
{
    public string Data { get; }

    private FactoryAlterSample(string data)
    {
        Data = data;
    }

    public static FactoryAlterSample Create(int data)
    {
        return new FactoryAlterSample("Value:" + data);
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
