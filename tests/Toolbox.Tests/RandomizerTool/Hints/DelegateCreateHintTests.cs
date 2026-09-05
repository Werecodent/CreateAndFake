using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class DelegateCreateHintTests : CreateHintTestBase<DelegateCreateHint>
{
    private static readonly Type[] _ActionTypes =
    [
        typeof(Action),
        typeof(Action<>),
        typeof(Action<,>),
        typeof(Action<,,>),
        typeof(Action<,,,>),
        typeof(Action<,,,,>),
        typeof(Action<,,,,,>),
        typeof(Action<,,,,,,>),
        typeof(Action<,,,,,,,>),
        typeof(Action<,,,,,,,,>),
        typeof(Action<,,,,,,,,,>),
        typeof(Action<,,,,,,,,,,>),
        typeof(Action<,,,,,,,,,,,>),
        typeof(Action<,,,,,,,,,,,,>),
        typeof(Action<,,,,,,,,,,,,,>),
        typeof(Action<,,,,,,,,,,,,,,>),
        typeof(Action<,,,,,,,,,,,,,,,>),
    ];

    private static readonly Type[] _FuncTypes =
    [
        typeof(Func<>),
        typeof(Func<,>),
        typeof(Func<,,>),
        typeof(Func<,,,>),
        typeof(Func<,,,,>),
        typeof(Func<,,,,,>),
        typeof(Func<,,,,,,>),
        typeof(Func<,,,,,,,>),
        typeof(Func<,,,,,,,,>),
        typeof(Func<,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,,>),
        typeof(Func<,,,,,,,,,,,,,,,,>),
    ];

    private static readonly Type[] _ValidTypes =
    [
        typeof(Action<string, object, int>),
        typeof(Func<int, string, object>),
        typeof(Delegate),
        typeof(Action),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DelegateCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Fact]
    internal static void Create_HandlesAllDelegates()
    {
        foreach (Type type in _ActionTypes.Concat(_FuncTypes))
        {
            Tools.Randomizer.Create(type).Assert().IsNotNull();
        }
    }

    [Fact]
    internal void Create_HandlesOutRef()
    {
        CreateHintResult result = TestInstance.TryToCreate(
            typeof(Action<IOutRef>),
            CreateChainer()
        );
        result.HasData.Assert().Is(true);

        Action<IOutRef> action = (Action<IOutRef>)result.Data;

        OutRef<int> sampleInt = new();
        action.Invoke(sampleInt);
        sampleInt.Var.Assert().IsNot(default(int));

        OutRef<string> sampleString = new();
        action.Invoke(sampleString);
        sampleString.Var.Assert().IsNot(default(string));
    }
}
