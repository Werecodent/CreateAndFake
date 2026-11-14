namespace Werecodent.CreateAndFake.Samples.ErrorCases;

[InvalidSample]
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
