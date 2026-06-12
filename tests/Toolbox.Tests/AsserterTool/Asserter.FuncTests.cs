using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.AsserterTool;

public sealed class AsserterFuncTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal void Func_HasResultReturnsResult(int num)
    {
        _testInstance.HasResult(() => num).Assert().Is(num);
    }
}
