using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool.CreateHints;

namespace CreateAndFake.Tests.RandomizerTool.CreateHints;

public sealed class ExceptionCreateHintTests : CreateHintTestBase<ExceptionCreateHint>
{
    private static readonly ExceptionCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Exception)];

    private static readonly Type[] _InvalidTypes = [typeof(object), typeof(FakeVerifyException)];

    public ExceptionCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
