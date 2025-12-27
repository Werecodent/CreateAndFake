using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Tests.TesterTool.TestSamples;

public static class LongMethodSample
{
    [ExcludeFromCodeCoverage]
    public static void BeSlow<T>(string data, out T output)
        where T : new()
    {
        Thread.Sleep(new TimeSpan(0, 0, 3));
        Thread.Sleep(new TimeSpan(0, 0, 2));
        Thread.Sleep(new TimeSpan(0, 0, 1));

        output = new T();
        throw new InvalidOperationException($"{nameof(BeSlow)} finished with data: {data}.");
    }
}
