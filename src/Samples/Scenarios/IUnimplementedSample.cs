namespace CreateAndFake.Samples.Scenarios;

public interface IUnimplementedSample
{
    int Flag { get; }

    bool Funny { set; }

    string GetData();
}
