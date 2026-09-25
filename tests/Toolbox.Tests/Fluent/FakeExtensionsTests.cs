using Werecodent.CreateAndFake.FakerTool;
using FakeExtensions = Werecodent.CreateAndFake.Fluent.FakeExtensions;

namespace Werecodent.CreateAndFake.Tests.Fluent;

#pragma warning disable RCS1021 // Expression-bodied lambda creates incorrect type.

public static class FakeExtensionsTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(InvalidOperationException), typeof(InvalidCastException)],
        };

    [Fact]
    internal static Task FakeExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(FakeExtensions),
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task FakeExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(FakeExtensions),
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Theory, RandomData]
    internal static void SetupReturn_WorksForVoidMethods([Fake] IDisposable disposable)
    {
        bool called = false;

        disposable.SetupReturn(
            x => x.Dispose(),
            Behavior.Call(() =>
            {
                called = true;
            })
        );

        disposable.Dispose();
        called.Assert().Is(true);
    }
}

#pragma warning restore
