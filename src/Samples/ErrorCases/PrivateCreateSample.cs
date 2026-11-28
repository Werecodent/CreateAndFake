using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample, ExcludeFromCreateAndFake]
public sealed class PrivateCreateSample
{
    private PrivateCreateSample() { }
}
