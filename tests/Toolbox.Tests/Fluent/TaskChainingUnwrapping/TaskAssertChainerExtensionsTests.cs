using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public sealed class TaskAssertChainerExtensionsTests()
    : AutoTester(typeof(TaskAssertChainerExtensions))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(AssertException)] };

    [Theory, RandomData]
    internal static Task And_Chains(Task<AssertChainer<AssertAction>> chainer)
    {
        return chainer.And().Pass();
    }
}
