using System.Collections.Frozen;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Design.Tests.Randomization;

public abstract class ValueRandomTestBase<T>
    where T : ValueRandom
{
    private static readonly FrozenSet<Type> ignorableExceptions =
    [
        typeof(NotSupportedException),
        typeof(ArgumentOutOfRangeException),
        typeof(OverflowException),
    ];

    private static readonly ValueRandom _TestInstance = Tools.Randomizer.Create<T>();

    [Fact]
    public Task ValueRandom_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<T>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = ignorableExceptions }
        );
    }

    [Fact]
    public Task ValueRandom_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<T>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = ignorableExceptions }
        );
    }

    [Fact]
    public void Supports_InvalidTypeFalse()
    {
        _TestInstance.Supports(typeof(object)).Assert().Is(false);
    }

    [Fact]
    public void Supports_Int()
    {
        TestBasicSupport<int>(default);
        TestNextRange(int.MinValue, int.MaxValue, v => v + 1, v => v - 1);
        TestNextOverflow(int.MinValue / 2 - 1, int.MaxValue / 2 + 1);
    }

    [Fact]
    public void Supports_UInt()
    {
        TestBasicSupport<uint>(default);
        TestNextRange(uint.MinValue, uint.MaxValue, v => v + 1, v => v - 1);
    }

    [Fact]
    public void Supports_Long()
    {
        TestBasicSupport<long>(default);
        TestNextRange(long.MinValue, long.MaxValue, v => v + 1, v => v - 1);
        TestNextOverflow(long.MinValue / 2 - 1, long.MaxValue / 2 + 1);
    }

    [Fact]
    public void Supports_ULong()
    {
        TestBasicSupport<ulong>(default);
        TestNextRange(ulong.MinValue, ulong.MaxValue, v => v + 1, v => v - 1);
    }

    [Fact]
    public void Supports_Short()
    {
        TestBasicSupport<short>(default);
        TestNextRange(short.MinValue, short.MaxValue, v => (short)(v + 1), v => (short)(v - 1));
        TestNextOverflow((short)(short.MinValue / 2 - 1), (short)(short.MaxValue / 2 + 1));
    }

    [Fact]
    public void Supports_UShort()
    {
        TestBasicSupport<ushort>(default);
        TestNextRange(ushort.MinValue, ushort.MaxValue, v => (ushort)(v + 1), v => (ushort)(v - 1));
    }

    [Fact]
    public void Supports_Byte()
    {
        TestBasicSupport<byte>(default);
        TestNextRange(byte.MinValue, byte.MaxValue, v => (byte)(v + 1), v => (byte)(v - 1));
    }

    [Fact]
    public void Supports_SByte()
    {
        TestBasicSupport<sbyte>(default);
        TestNextRange(sbyte.MinValue, sbyte.MaxValue, v => (sbyte)(v + 1), v => (sbyte)(v - 1));
        TestNextOverflow((sbyte)(sbyte.MinValue / 2 - 1), (sbyte)(sbyte.MaxValue / 2 + 1));
    }

    [Fact]
    public void Supports_Bool()
    {
        TestBasicSupport<bool>(default);
        TestNext(false, true);
        _TestInstance.Assert(t => t.Next(true, false)).Throws<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Supports_Char()
    {
        TestBasicSupport<char>(default);
        TestNextRange(char.MinValue, char.MaxValue, v => (char)(v + 1), v => (char)(v - 1));
    }

    [Fact]
    public void Supports_Float()
    {
        TestBasicSupport<float>(default);
        TestNextRange(float.MinValue, float.MaxValue, float.BitIncrement, float.BitDecrement);
        TestNextOverflow(float.MinValue / 2 - 1, float.MaxValue / 2 + 1);
    }

    [Fact]
    public void Supports_Double()
    {
        TestBasicSupport<double>(default);
        TestNextRange(double.MinValue, double.MaxValue, double.BitIncrement, double.BitDecrement);
        TestNextOverflow(double.MinValue / 2 - 1, double.MaxValue / 2 + 1);
    }

    [Fact]
    public void Supports_Decimal()
    {
        TestBasicSupport<decimal>(default);
        TestNextRange(decimal.MinValue, decimal.MaxValue, v => v + 1, v => v - 1);
        TestNextOverflow(decimal.MinValue / 2 - 1, decimal.MaxValue / 2 + 1);
    }

    private static void TestBasicSupport<TValueType>(TValueType zero)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        _TestInstance.Supports<TValueType>().Assert().Is(true);
        _TestInstance.Supports(typeof(TValueType)).Assert().Is(true);
        _TestInstance.Next<TValueType>().Assert().IsNotNull();
        _TestInstance.Next(typeof(TValueType)).Assert().IsNotNull();
        _TestInstance.Next(zero).Assert().Is(zero);

        TValueType sample = _TestInstance.Next<TValueType>();
        Limiter.Hundred.StallUntil(
            "Variance testing.",
            () => !sample.Equals(_TestInstance.Next<TValueType>())
        );
    }

    private static void TestNextRange<TValueType>(
        TValueType min,
        TValueType max,
        Func<TValueType, TValueType> addSome,
        Func<TValueType, TValueType> subtractSome
    )
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        TestNext(min, max);
        TestNext(min, addSome(min));
        TestNext(subtractSome(max), max);

        min.Assert(m => _TestInstance.Next(max, m)).Throws<ArgumentOutOfRangeException>();

        Limiter.Myriad.Repeat(
            "Minimal range adherence testing.",
            () =>
            {
                TValueType sample = _TestInstance.Next(addSome(min), max);
                TestNext(subtractSome(sample), sample);
                TestNext(sample, addSome(sample));
            }
        );

        Limiter.Myriad.Repeat(
            "Random range testing.",
            () =>
            {
                TValueType sample1 = _TestInstance.Next<TValueType>();
                TValueType sample2 = _TestInstance.Next<TValueType>();
                if (sample1.CompareTo(sample2) < 0)
                {
                    TestNext(sample1, sample2);
                }
                else if (sample1.CompareTo(sample2) > 0)
                {
                    TestNext(sample2, sample1);
                }
            }
        );
    }

    private static void TestNextOverflow<TValueType>(TValueType halfMin, TValueType halfMax)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        Limiter.Myriad.Repeat("Potential overflow testing.", () => TestNext(halfMin, halfMax));
    }

    private static void TestNext<TValueType>(TValueType min, TValueType max)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        min.Assert().LessThan(max, "Difference was too small for next randomization.");
        _TestInstance.Next(min, max).Assert().GreaterThanOrEqualTo(min).And.LessThan(max);
    }

    [Theory, RandomData]
    public void Next_UnsupportedTypeThrows(StructSample sample)
    {
        _TestInstance.Assert(t => t.Next<StructSample>()).Throws<NotSupportedException>();
        _TestInstance.Assert(t => t.Next(typeof(StructSample))).Throws<NotSupportedException>();
        _TestInstance.Assert(t => t.Next(sample)).Throws<NotSupportedException>();
    }

    [Fact]
    public void Next_MaxDoubleExcluded()
    {
        const double min = 9.9999999;
        const double max = 10;

        for (int i = 0; i < 25000; i++)
        {
            _TestInstance.Next(min, max).Assert().GreaterThanOrEqualTo(min).And.LessThan(max);
        }
    }

    [Fact]
    public void Next_MaxDecimalExcluded()
    {
        const decimal min = 9.9999999M;
        const decimal max = 10;

        for (int i = 0; i < 25000; i++)
        {
            _TestInstance.Next(min, max).Assert().GreaterThanOrEqualTo(min).And.LessThan(max);
        }
    }

    [Theory, RandomData]
    public void NextItem_CollectionsWork(ICollection<string> data)
    {
        data.Assert().Contains(_TestInstance.NextItem(data));
    }

    [Theory, RandomData]
    public void NextItem_ReadOnlyCollectionsWork(IReadOnlyCollection<string> data)
    {
        data.Assert().Contains(_TestInstance.NextItem(data));
    }

    [Fact]
    public void NextItem_YieldWorks()
    {
        _TestInstance.NextItem(CreateSeries(1)).Assert().IsNot(null);
        _TestInstance.NextItem(CreateSeries(2)).Assert().IsNot(null);
        _TestInstance.NextItem(CreateSeries(3)).Assert().IsNot(null);
    }

    [Fact]
    public void NextItem_EmptyThrows()
    {
        _TestInstance.Assert(t => t.NextItem(CreateSeries(0))).Throws<InvalidOperationException>();
        _TestInstance
            .Assert(t => t.NextItem(Array.Empty<object>()))
            .Throws<InvalidOperationException>();
    }

    [Fact]
    public void NextItemOrDefault_EmptyGivesDefault()
    {
        _TestInstance.NextItemOrDefault((int[])null).Assert().Is(0);
        _TestInstance.NextItemOrDefault(CreateSeries(0)).Assert().Is(null);
        _TestInstance.NextItemOrDefault((object[])null).Assert().Is(null);
        _TestInstance.NextItemOrDefault(Array.Empty<object>()).Assert().Is(null);
    }

    private static IEnumerable<object> CreateSeries(int size)
    {
        for (int i = 0; i < size; i++)
        {
            yield return new object();
        }
    }
}
