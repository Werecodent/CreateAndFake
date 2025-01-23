using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Toolbox.FakerTool;

#pragma warning disable IDE0060 // Remove unused parameter: For testing.
#pragma warning disable CS9113 // Parameter is unread: For testing.

namespace CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "For testing.")]
public sealed class MockDisposableSample(object value) : IDisposable
{
    internal static readonly object _Lock = new();

    internal static Fake<IDisposable> _Fake = Tools.Faker.Stub<IDisposable>();

    internal static int _ClassDisposes = 0;

    internal static int _FinalizerDisposes = 0;

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Must ignore in finalizer.")]
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

#pragma warning restore CS9113 // Parameter is unread
#pragma warning restore IDE0060 // Remove unused parameter
