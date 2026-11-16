using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample, ExcludeFromCreateAndFake]
public interface IUnimplementedSample
{
    int Flag { get; }

    bool Funny { set; }

    string GetData();
}
