using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool.TestSamples;

#pragma warning disable // For testing.

public sealed class MockDisposableSample(object value) : IDisposable
{
    internal static readonly SemaphoreSlim _Lock = new(1);

    internal static Fake<IDisposable> _Fake = Tools.Faker.Stub<IDisposable>();

    internal static int _ClassDisposes = 0;

    internal static int _FinalizerDisposes = 0;

    [ExcludeFromCodeCoverage]
    ~MockDisposableSample()
    {
        try
        {
            _FinalizerDisposes++;
        }
        catch { }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _ClassDisposes++;
    }

    public IDisposable DisposePass(object value)
    {
        return _Fake.Dummy;
    }
}

#pragma warning restore
