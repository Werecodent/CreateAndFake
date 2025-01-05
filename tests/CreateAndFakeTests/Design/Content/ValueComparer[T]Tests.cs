using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFakeTests.Design.Content;

public static class ValueComparer_T_Tests
{
    [Fact]
    internal static void ValueComparer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(ValueComparer<>));
    }

    [Fact]
    internal static void ValueComparer_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(ValueComparer<>));
    }

    [Theory, RandomData]
    internal static void GetHashCode_UsesGetValueHash([Fake] IValueEquatable stub, int hash)
    {
        stub.GetValueHash().SetupReturn(hash, 3);
        ValueComparer<IValueEquatable>.Use.GetHashCode(stub).Assert().Is(hash);
        ValueComparer<IValueEquatable>.Use.GetHashCode([stub]).Assert().IsNot(0);
        ValueComparer<IValueEquatable>.Use.GetHashCode(MapByIndex([stub])).Assert().IsNot(0);
        stub.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Equals_UsesValuesEqual(
        [Fake] IValueEquatable stub1, [Fake] IValueEquatable stub2, bool result)
    {
        stub1.ValuesEqual(stub2).SetupReturn(result);
        stub2.ValuesEqual(stub1).SetupReturn(result);

        ValueComparer<IValueEquatable>.Use.Equals(stub1, stub2).Assert().Is(result);
        ValueComparer<IValueEquatable>.Use.Equals([stub1], [stub2]).Assert().Is(result);
        ValueComparer<IValueEquatable>.Use.Equals(MapByIndex([stub1]), MapByIndex([stub2])).Assert().Is(result);
    }

    [Theory, RandomData]
    internal static void Equals_UsesGetValueHash(
        [Fake] IValueEquatable stub, [Fake] IValueEquatable equalStub, [Fake] IValueEquatable unequalStub, int hash)
    {
        stub.GetValueHash().SetupReturn(hash);
        equalStub.GetValueHash().SetupReturn(hash);
        unequalStub.GetValueHash().SetupReturn(hash.CreateVariant());

        ValueComparer<IValueEquatable> comparer = ValueComparer<IValueEquatable>.Use;

        comparer.Compare(stub, stub).Assert().Is(0);
        comparer.Compare(stub, equalStub).Assert().Is(0);
        comparer.Compare(stub, unequalStub).Assert().IsNot(0);
        comparer.Compare(stub, null).Assert().IsNot(0);
        comparer.Compare(null, stub).Assert().IsNot(0);
        comparer.Compare((IValueEquatable)null, null).Assert().Is(0);
    }

    private static Dictionary<int, T> MapByIndex<T>(IEnumerable<T> collection)
    {
        return collection
            .Select((item, index) => new { index, item })
            .ToDictionary(pair => pair.index, pair => pair.item);
    }
}
