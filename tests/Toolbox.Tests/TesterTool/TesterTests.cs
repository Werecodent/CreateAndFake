using System.Reflection;
using CreateAndFake.FakerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.Tests.TesterTool.TestSamples;

namespace CreateAndFake.Tests.TesterTool;

public static class TesterTests
{
    private static readonly Tester _ShortTestInstance = new(
        Tools.Tester.Options with
        {
            Timeout = new TimeSpan(0, 0, 0, 0, 100),
        }
    );

    private static readonly Tester _LongTestInstance = new(
        Tools.Tester.Options with
        {
            Timeout = new TimeSpan(0, 0, 10),
        }
    );

    [Fact]
    internal static void Tester_AllMethodsVirtual()
    {
        Tools.Asserter.IsEmpty(
            typeof(Tester)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsVirtual)
                .Select(m => m.Name)
                .Where(n => n != $"get_{nameof(Tester.Options)}")
        );
    }

    [Fact]
    internal static async Task Tester_GuardsNulls()
    {
        Type nullType = null;
        await nullType
            .Assert(t => _ShortTestInstance.PreventsNullRefException(t))
            .Throws<ArgumentNullException>();
        await nullType
            .Assert(t => _ShortTestInstance.PreventsParameterMutation(t))
            .Throws<ArgumentNullException>();
    }

    [Fact]
    internal static async Task PreventsNullRefException_Disposes()
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            await _LongTestInstance.PreventsNullRefException<MockDisposableSample>();
            Tools.Asserter.Is(2, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Exactly(2), d => d.Dispose());
        }
        finally
        {
            MockDisposableSample._Lock.Release();
        }
    }

    [Fact]
    internal static async Task PreventsParameterMutation_Disposes()
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            await _LongTestInstance.PreventsParameterMutation<MockDisposableSample>();
            Tools.Asserter.Is(2, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Exactly(2), d => d.Dispose());
        }
        finally
        {
            MockDisposableSample._Lock.Release();
        }
    }
}
