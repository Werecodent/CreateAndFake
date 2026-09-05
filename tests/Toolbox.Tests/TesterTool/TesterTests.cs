using System.Reflection;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.TesterTool;
using Werecodent.CreateAndFake.Tests.TesterTool.TestSamples;

namespace Werecodent.CreateAndFake.Tests.TesterTool;

public static class TesterTests
{
    private static readonly Tester _ShortTestInstance = new(
        Tools.Tester.Options with
        {
            Runner = new Runner(
                Tools.Runner.Options with
                {
                    Timeout = new TimeSpan(0, 0, 0, 0, 100),
                }
            ),
        }
    );

    private static readonly Tester _LongTestInstance = new(
        Tools.Tester.Options with
        {
            Runner = new Runner(Tools.Runner.Options with { Timeout = new TimeSpan(0, 0, 10) }),
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
        await _ShortTestInstance
            .PreventsNullRefExceptionAsync(nullType, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<ArgumentNullException>(TestContext.Current.CancellationToken);
        await _ShortTestInstance
            .PreventsParameterMutationAsync(nullType, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<ArgumentNullException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static async Task PreventsNullRefExceptionAsync_Disposes()
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            await _LongTestInstance.PreventsNullRefExceptionAsync<MockDisposableSample>(
                TestContext.Current.CancellationToken,
                opt =>
                    opt with
                    {
                        DisableNullRefExceptionTests = false,
                        IncludeConstructors = true,
                        IncludeInstanceMethods = true,
                    }
            );
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
    internal static async Task PreventsParameterMutationAsync_Disposes()
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            await _LongTestInstance.PreventsParameterMutationAsync<MockDisposableSample>(
                TestContext.Current.CancellationToken,
                opt =>
                    opt with
                    {
                        DisableParameterMutationTests = false,
                        IncludeConstructors = true,
                        IncludeInstanceMethods = true,
                    }
            );
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
