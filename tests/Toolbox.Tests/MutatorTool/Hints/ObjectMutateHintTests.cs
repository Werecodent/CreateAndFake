using System.Collections;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Hints;
using CreateAndFake.Samples.BasicData;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public sealed class ObjectMutateHintTests : MutateHintTestBase<ObjectMutateHint>
{
    public ObjectMutateHintTests()
        : base([typeof(DataHolderSample)], [typeof(ICollection)]) { }

    [Fact]
    internal void SupportedTypes_SupportsObject()
    {
        TestInstance.SupportedTypes.Assert().Is(new Type[] { typeof(object) });
    }

    [Fact]
    internal void Modify_StatelessImmutable()
    {
        RunModifyTest<StatelessSample>(false);
    }

    [Fact]
    internal void Modify_ComplexObjectsWork()
    {
        RunModifyTest<ChildWithParentSample>(true);
        RunModifyTest<DataHolderSample>(true);
        RunModifyTest<DataSample>(true);
        RunModifyTest<CompleteDto>(true);
        RunModifyTest<FieldSample>(true);
    }

    [Fact]
    internal void Modify_IgnoresReadOnly()
    {
        TestInstance
            .TryToModify(new AllInvalidsSample(), CreateChainer())
            .Assert()
            .Is(new MutateHintResult(false));
    }

    [Fact]
    internal void Modify_RandomizesProperty()
    {
        PropertyLoopSample original = Tools.Randomizer.Create<PropertyLoopSample>();
        IMutatorChainer chainer = CreateChainer();
        MutateHintResult expectedResult = new(true);

        Limiter.Myriad.Repeat(
            "Test smart randomization unequal.",
            () =>
            {
                PropertyLoopSample sample = new() { FirstName = original.FirstName };
                TestInstance.TryToModify(sample, chainer).Assert().Is(expectedResult);
                sample.FirstName.Assert().IsNot(original.FirstName);
            },
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal void Modify_RandomizesField()
    {
        FieldLoopSample original = Tools.Randomizer.Create<FieldLoopSample>();
        IMutatorChainer chainer = CreateChainer();
        MutateHintResult expectedResult = new(true);

        Limiter.Myriad.Repeat(
            "Test smart randomization unequal.",
            () =>
            {
                FieldLoopSample sample = new() { LastName = original.LastName };
                TestInstance.TryToModify(sample, chainer).Assert().Is(expectedResult);
                sample.LastName.Assert().IsNot(original.LastName);
            },
            TestContext.Current.CancellationToken
        );
    }

    private sealed class PropertyLoopSample
    {
        public string FirstName { get; set; }
    }

    private sealed class FieldLoopSample
    {
        public string LastName;
    }

#pragma warning disable IDE0052, S1144, S2325, S2376 // For testing.
    private sealed class AllInvalidsSample
    {
        public const int ConstField = 0;

        public readonly string ReadOnlyField = "Value";

        private string _hidden = "";

        public string ReadOnlyProp { get; } = "Value";

        public string SetOnlyProp
        {
            set => _hidden = value;
        }

        public string BadProp
        {
            get => "";
            set => throw new NotSupportedException();
        }
    }
#pragma warning restore S1144, IDE0052, S2376, S2325
}
