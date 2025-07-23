namespace CreateAndFake.Samples.ErrorCases;

[InvalidSample]
public interface IUnimplementedSample
{
    int Flag { get; }

    bool Funny { set; }

    string GetData();
}
