using System.Collections;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class UnwrapperTests
{
    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsIntTaskResults(int value)
    {
        Task<int> run = Task.Run(() => value);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(value);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsStringTaskResults(string value)
    {
        Task<string> run = Task.Run(() => value);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(value);
    }

    [Fact]
    internal static async Task UnwrapResult_ReturnsNullTaskResults()
    {
        Task<object> run = Task.Run(() => (object)null);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(null);
    }

    [Fact]
    internal static async Task UnwrapResult_ReturnsTaskResults()
    {
        (await Unwrapper.UnwrapResult(() => Task.CompletedTask)).Assert().Is(VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsIntValueTaskResults(int value)
    {
        ValueTask<int> run = ValueTask.FromResult(value);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(value);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsStringValueTaskResults(string value)
    {
        ValueTask<string> run = ValueTask.FromResult(value);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(value);
    }

    [Fact]
    internal static async Task UnwrapResult_ReturnsNullValueTaskResults()
    {
        ValueTask<object> run = ValueTask.FromResult<object>(null);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(null);
    }

    [Fact]
    internal static async Task UnwrapResult_ReturnsValueTaskResults()
    {
        (await Unwrapper.UnwrapResult(() => ValueTask.CompletedTask))
            .Assert()
            .Is(VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsStringResults(string value)
    {
        (await Unwrapper.UnwrapResult(() => value)).Assert().Is(value);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsIntResults(int value)
    {
        (await Unwrapper.UnwrapResult(() => value)).Assert().Is(value);
    }

    [Fact]
    internal static async Task UnwrapResult_ReturnsNullResults()
    {
        (await Unwrapper.UnwrapResult(() => null)).Assert().Is(null);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsListIntResults(List<int> value)
    {
        (await Unwrapper.UnwrapResult(() => value)).Assert().Is(value);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsListStringResults(List<string> value)
    {
        (await Unwrapper.UnwrapResult(() => value)).Assert().Is(value);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsArrayIntResults(int[] value)
    {
        (await Unwrapper.UnwrapResult(() => value)).Assert().Is(value);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsArrayStringResults(string[] value)
    {
        (await Unwrapper.UnwrapResult(() => value)).Assert().Is(value);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_ReturnsCollectionResults(ICollection value)
    {
        (await Unwrapper.UnwrapResult(() => value)).Assert().Is(value);
    }
}
