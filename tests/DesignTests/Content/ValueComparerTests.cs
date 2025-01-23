using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFake.DesignTests.Content;

#pragma warning disable CA1859 // Change to concrete types: Needed for generic resolution.

public static class ValueComparerTests
{
    private static readonly int[] _OneValue = [0];

    private static readonly int[] _TwoValues = [0, 0];

    [Fact]
    internal static void ValueComparer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<ValueComparer>();
    }

    [Fact]
    internal static void ValueComparer_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<ValueComparer>();
    }

    [Fact]
    internal static void Equals_MismatchSizeFalse()
    {
        ValueComparer.Use.Equals(_OneValue, _TwoValues).Assert().Is(false);
        ValueComparer.Use.Equals(_TwoValues, _OneValue).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void GetHashCode_SupportsParams(int item1, int item2, int item3)
    {
        Tools.Asserter.Is(
            ValueComparer.Use.GetHashCode(new List<int> { item1, item2, item3 }),
            ValueComparer.Use.GetHashCode(item1, item2, item3));
    }

    [Theory, RandomData]
    internal static void Equals_SupportsValueEquatable(
        [Fake] IValueEquatable stub1, [Fake] IValueEquatable stub2, bool result)
    {
        stub1.ValuesEqual(stub2).SetupReturn(result);
        stub2.ValuesEqual(stub1).SetupReturn(result);

        ValueComparer.Use.Equals(stub1, stub2).Assert().Is(result);
        ValueComparer.Use.Equals((object)stub1, stub2).Assert().Is(result);
    }

    [Theory, RandomData]
    internal static void Equals_SupportsValueEquatableNulls([Fake] IValueEquatable stub, bool result)
    {
        stub.ValuesEqual(null).SetupReturn(result, 2);
        ValueComparer.Use.Equals(stub, null).Assert().Is(result);
        ValueComparer.Use.Equals(null, stub).Assert().Is(result);
        ValueComparer.Use.Equals((IValueEquatable)null, null).Assert().Is(true);
        stub.Assert().Called();
    }

    [Theory, RandomData]
    internal static void GetHashCode_SupportsValueEquatable([Fake] IValueEquatable stub, int hash)
    {
        stub.GetValueHash().SetupReturn(hash, 2);
        ValueComparer.Use.GetHashCode(stub).Assert().Is(hash);
        ValueComparer.Use.GetHashCode((object)stub).Assert().Is(hash);
        stub.Assert().Called();
    }

    [Fact]
    internal static void ValueComparer_SupportsObject()
    {
        TestBehavior<int, object>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<string, object>(ValueComparer.Use, ValueComparer.Use);
    }

    [Fact]
    internal static void ValueComparer_SupportsIEnumerable()
    {
        TestBehavior<IEnumerable<int>, object>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<IEnumerable<int>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);

        TestBehavior<IEnumerable<string>, object>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<IEnumerable<string>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
    }

    [Fact]
    internal static void ValueComparer_SupportsIDictionary()
    {
        TestBehavior<IDictionary<int, int>, object>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<IDictionary<int, int>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<Dictionary<int, int>, IDictionary>(ValueComparer.Use, ValueComparer.Use);

        TestBehavior<IDictionary<string, int>, object>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<IDictionary<string, int>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<Dictionary<string, int>, IDictionary>(ValueComparer.Use, ValueComparer.Use);

        TestBehavior<IDictionary<int, string>, object>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<IDictionary<int, string>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<Dictionary<int, string>, IDictionary>(ValueComparer.Use, ValueComparer.Use);

        TestBehavior<IDictionary<string, string>, object>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<IDictionary<string, string>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
        TestBehavior<Dictionary<string, string>, IDictionary>(ValueComparer.Use, ValueComparer.Use);
    }

    private static void TestBehavior<TActual, TComparer>(
        IComparer<TComparer> comparer, IEqualityComparer<TComparer> equalityComparer) where TActual : TComparer
    {
        TActual baseObject = Tools.Randomizer.Create<TActual>();
        TActual equalObject = Tools.Duplicator.Copy(baseObject);
        TActual unequalObject = Tools.Mutator.Variant(baseObject);

        Tools.Asserter.Is(true, equalityComparer.Equals(default, default));
        Tools.Asserter.Is(true, equalityComparer.Equals(baseObject, baseObject));
        Tools.Asserter.Is(true, equalityComparer.Equals(baseObject, equalObject));
        Tools.Asserter.Is(false, equalityComparer.Equals(baseObject, unequalObject));
        Tools.Asserter.Is(false, equalityComparer.Equals(baseObject, default));
        Tools.Asserter.Is(false, equalityComparer.Equals(default, baseObject));

        Tools.Asserter.Is(0, comparer.Compare(default, default));
        Tools.Asserter.Is(0, comparer.Compare(baseObject, baseObject));
        Tools.Asserter.Is(0, comparer.Compare(baseObject, equalObject));
        Tools.Asserter.IsNot(0, comparer.Compare(baseObject, unequalObject));
        Tools.Asserter.IsNot(0, comparer.Compare(baseObject, default));
        Tools.Asserter.IsNot(0, comparer.Compare(default, baseObject));
    }
}

#pragma warning restore CA1859 // Change to concrete types
