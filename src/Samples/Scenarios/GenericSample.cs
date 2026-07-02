using CreateAndFake.Design.Types;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public abstract class GenericSample<TClass>
{
    public abstract string Run<TMethod>(TMethod input);

    public abstract TReturn Run<TMethod, TReturn>(TClass in1, TMethod in2)
        where TMethod : DataSample
        where TReturn : new();

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
