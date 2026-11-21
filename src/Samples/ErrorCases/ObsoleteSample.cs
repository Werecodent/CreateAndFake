using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample, ExcludeFromCreateAndFake]
public sealed class ObsoleteSample(string data)
{
    public string GrabData()
    {
        return data;
    }

    [Obsolete("For testing.")]
    public string IsObsolete()
    {
        throw new InvalidOperationException("Obsolete method called.");
    }
}
