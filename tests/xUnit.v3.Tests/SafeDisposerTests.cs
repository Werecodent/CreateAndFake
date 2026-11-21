using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public sealed class SafeDisposerTests() : AutoTester(typeof(SafeDisposer))
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt =>
            opt with
            {
                DisableParameterMutationTests = true,
                DisableToolSetSupportTests = true,
                InjectionValues =
                [
                    new List<object>()
                    {
                        Tools.Randomizer.Create<IDisposable>(),
                        Tools.Randomizer.Create<IAsyncDisposable>(),
                    }.AsEnumerable(),
                ],
            };
}
