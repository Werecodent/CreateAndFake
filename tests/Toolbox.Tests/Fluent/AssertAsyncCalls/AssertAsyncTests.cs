using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncTests
{
    //s[Fact]
    internal static Task AssertAsync_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertAsync>(opt =>
            opt with
            {
                IgnoreAllExceptions = true,
            }
        );
    }

    //[Fact]
    internal static Task AssertAsync_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertAsync>(opt =>
            opt with
            {
                IgnoreAllExceptions = true,
            }
        );
    }

    //[Theory, RandomData]
    internal static Task Throws_HandlesAsyncNoError(InvalidDataException error)
    {
        return error.Assert(async e => await WaitTest(e)).Throws<InvalidDataException>();
    }

    private static async Task<bool> WaitTest(InvalidDataException error)
    {
        await Task.Delay(0, TestContext.Current.CancellationToken);
        throw error;
    }
}
