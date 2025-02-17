namespace CreateAndFakeTests.Toolbox.AsserterTool.Fluent;

public sealed class AssertAsyncTests
{
    [Theory, RandomData]
    internal async Task Throws_HandlesAsyncNoError(InvalidDataException error)
    {
        await error.Assert(async e => await WaitTest(e)).Throws<InvalidDataException>();
    }

    private static async Task<bool> WaitTest(InvalidDataException error)
    {
        await Task.Delay(0);
        throw error;
    }
}
