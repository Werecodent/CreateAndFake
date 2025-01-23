using System.Diagnostics.CodeAnalysis;

namespace CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

public static class LongMethodSample
{
    [ExcludeFromCodeCoverage]
    public static void BeSlow<T>(string data, out T output) where T : new()
    {
        Thread.Sleep(new TimeSpan(0, 0, 2));

        output = new T();
        throw new InvalidOperationException(data);
    }
}
