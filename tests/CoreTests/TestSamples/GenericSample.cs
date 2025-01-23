namespace CreateAndFakeTests.TestSamples;

public abstract class GenericSample<TClass>
{
    public abstract string Run<TMethod>(TMethod input);

    public abstract TReturn Run<TMethod, TReturn>(TClass in1, TMethod in2)
       where TMethod : DataSample
       where TReturn : new();
}
