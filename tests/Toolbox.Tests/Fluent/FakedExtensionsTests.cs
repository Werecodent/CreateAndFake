using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.Fluent;

#pragma warning disable RCS1021 // Expression-bodied lambda creates incorrect type.

public sealed class FakedExtensionsTests() : AutoTester(typeof(FakedExtensions))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt =>
            opt with
            {
                DisableParameterMutationTests = true,
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
