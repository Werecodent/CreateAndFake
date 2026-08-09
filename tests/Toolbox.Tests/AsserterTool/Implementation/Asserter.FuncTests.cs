using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.AsserterTool.Implementation;

public sealed class AsserterFuncTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal void HasResult_ReturnsResult(int num)
    {
        _testInstance.HasResult(() => num).Assert().Is(num);
    }
}
