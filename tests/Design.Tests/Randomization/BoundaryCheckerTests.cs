using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization;

public sealed class BoundaryCheckerTests() : AutoTester(typeof(BoundaryChecker))
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt =>
            opt with
            {
                DisableNullRefExceptionTests = true,
                IgnorableExceptions =
                [
                    typeof(ArgumentException),
                    typeof(InvalidCastException),
                    typeof(ArgumentOutOfRangeException),
                ],
            };
}
