using Werecodent.CreateAndFake.FakerTool;
using FakeExtensions = Werecodent.CreateAndFake.Fluent.FakeExtensions;

namespace Werecodent.CreateAndFake.Tests.Fluent;

#pragma warning disable RCS1021 // Expression-bodied lambda creates incorrect type.

public sealed class FakeExtensionsTests() : AutoTester(typeof(FakeExtensions))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                IgnorableExceptions =
                [
                    typeof(InvalidOperationException),
                    typeof(InvalidCastException),
                ],
            };

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
