using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public static class IsBadSampleTests
{
    [Fact]
    public static Task IsBadSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<IsBadSample>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(NotImplementedException)],
                    Randomizer = opt.Randomizer.WithOptions(o =>
                        o with
                        {
                            ContentRandomizationRequired = false,
                        }
                    ),
                }
        );
    }

    [Fact]
    public static Task IsBadSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<IsBadSample>(
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions = [typeof(NotImplementedException)],
                    Randomizer = opt.Randomizer.WithOptions(o =>
                        o with
                        {
                            ContentRandomizationRequired = false,
                        }
                    ),
                }
        );
    }
}
