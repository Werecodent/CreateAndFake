using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample, ExcludeFromCreateAndFake]
public interface IOnlyMockSample
{
    bool FailIfNotMocked();
}
