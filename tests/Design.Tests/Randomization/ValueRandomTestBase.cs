using System.Collections.Frozen;
using System.Numerics;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Samples.Scenarios;
#if NET5_0_OR_GREATER
using System.Text;
#endif

namespace Werecodent.CreateAndFake.Design.Tests.Randomization;

public abstract class ValueRandomTestBase<T>(T testInstance)
    where T : ValueRandom
{
    private static readonly FrozenSet<Type> _IgnorableExceptions =
    [
        typeof(ArgumentException),
        typeof(ArgumentOutOfRangeException),
        typeof(IterationLimitException),
        typeof(UnsupportedException),
        typeof(InvalidCastException),
    ];

    [Fact]
    public Task ValueRandom_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<T>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = _IgnorableExceptions }
        );
    }

    [Fact]
    public Task ValueRandom_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<T>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = _IgnorableExceptions }
        );
    }

    [Fact]
    public void NextBytes_PreventsNegatives()
    {
        Tools
            .Gen.Next(short.MinValue, (short)-1)
            .Assert(testInstance.NextBytes)
            .Throws<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Supports_InvalidTypeFalse()
    {
        testInstance.Supports(typeof(object)).Assert().Is(false);
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
    public void Supports_Char()
    {
        TestBasicSupport<char>(default);
        TestNextRange(char.MinValue, char.MaxValue, v => (char)(v + 1), v => (char)(v - 1));
    }

    [Fact]
    public void Supports_Decimal()
    {
        TestBasicSupport<decimal>(default);
        TestNextRange(decimal.MinValue, decimal.MaxValue, v => v + 1m, v => v - 1m);
        TestNextOverflow(decimal.MinValue / 2 - 1, decimal.MaxValue / 2 + 1);
    }

    [Fact]
    public void Supports_Float()
    {
        TestBasicSupport<float>(default);
        TestNextRange(float.MinValue, float.MaxValue, v => v + 1f, v => v - 1f);
        TestNextOverflow(float.MinValue / 2 - 1, float.MaxValue / 2 + 1);
    }

    [Fact]
    public void Supports_Double()
    {
        TestBasicSupport<double>(default);
        TestNextRange(double.MinValue, double.MaxValue, v => v + 1d, v => v - 1d);
        TestNextOverflow(double.MinValue / 2 - 1, double.MaxValue / 2 + 1);
    }

    [Fact]
    public void Supports_BigInteger()
    {
        BigInteger min = new(long.MinValue);
        BigInteger max = new(long.MaxValue);

        TestBasicSupport<BigInteger>(default);
        TestNextRange(min, max, v => v + 1, v => v - 1);
        TestNextOverflow(min / 2 - 1, max / 2 + 1);
    }

    [Fact]
    public void Supports_TimeSpan()
    {
        TestBasicSupport<TimeSpan>(default);
        TestNextRange(
            TimeSpan.MinValue,
            TimeSpan.MaxValue,
            v => v.Add(TimeSpan.FromTicks(1)),
            v => v.Subtract(TimeSpan.FromTicks(1))
        );
        TestNextOverflow(
            TimeSpan.FromTicks(TimeSpan.MinValue.Ticks / 2 - 1),
            TimeSpan.FromTicks(TimeSpan.MaxValue.Ticks / 2 + 1)
        );
    }

    [Fact]
    public void Supports_DateTime()
    {
        TestBasicSupport<DateTime>(default);
        TestNextRange(
            DateTime.MinValue,
            DateTime.MaxValue,
            v => v.AddTicks(1),
            v => v.AddTicks(-1)
        );
    }

    [Fact]
    public void Supports_DateTimeOffset()
    {
        TestNext(DateTimeOffset.MinValue, DateTimeOffset.MinValue);
        TestNext(DateTimeOffset.MaxValue.AddTicks(-61), DateTimeOffset.MaxValue.AddTicks(-60));
        TestBasicSupport<DateTimeOffset>(default);
        TestNextRange(
            DateTimeOffset.MinValue,
            DateTimeOffset.MaxValue,
            v => v.AddTicks(1),
            v => v.AddTicks(-1)
        );
    }

#if NET6_0_OR_GREATER
    [Fact]
    public void Supports_TimeOnly()
    {
        TestBasicSupport<TimeOnly>(default);
        TestNextRange(
            TimeOnly.MinValue,
            TimeOnly.MaxValue,
            v => v.Add(TimeSpan.FromTicks(1)),
            v => v.Add(TimeSpan.FromTicks(-1))
        );
    }

    [Fact]
    public void Supports_DateOnly()
    {
        TestBasicSupport<DateOnly>(default);
        TestNextRange(DateOnly.MinValue, DateOnly.MaxValue, v => v.AddDays(1), v => v.AddDays(-1));
    }
#endif

#if NET5_0_OR_GREATER
    [Fact]
    public void Supports_Half()
    {
        TestBasicSupport<Half>(default);
        TestNextRange(Half.MinValue, Half.MaxValue, v => v + (Half)1f, v => v - (Half)1f);
        TestNextOverflow(Half.MinValue / (Half)2 - (Half)1, Half.MaxValue / (Half)2 + (Half)1);
    }

    [Fact]
    public void Supports_Rune()
    {
        TestBasicSupport<Rune>(default);
        TestNext(new Rune(0x0000), new Rune(0x10FFFF));
        TestNextRange(
            new Rune(0x0000),
            new Rune(0xD7FF),
            v => new Rune(v.Value + 1),
            v => new Rune(v.Value - 1)
        );
        TestNextRange(
            new Rune(0xE000),
            new Rune(0x10FFFF),
            v => new Rune(v.Value + 1),
            v => new Rune(v.Value - 1)
        );
        TestNext(new Rune(0xD7FF), new Rune(0xE000));
    }
#endif

    [Fact]
    public void Supports_Bool()
    {
        TestBasicSupport<bool>(default);
        TestNext(false, true);

        bool sample = testInstance.Next(false, true);
        Limiter.Hundred.StallUntil(
            "Variance testing.",
            () => sample != testInstance.Next(false, true),
            TestContext.Current.CancellationToken
        );

        testInstance.Next(false, false).Assert().Is(false);
        testInstance.Next(true, true).Assert().Is(true);
        testInstance.Assert(x => x.Next(true, false)).Throws<ArgumentOutOfRangeException>();
    }

    private void TestBasicSupport<TValueType>(TValueType zero)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        testInstance.Supports<TValueType>().Assert().Is(true);
        testInstance.Supports(typeof(TValueType)).Assert().Is(true);
        testInstance.Next<TValueType>().Assert().IsNotNull();
        testInstance.Next(typeof(TValueType)).Assert().IsNotNull();
        testInstance.Next(zero).Assert().Is(zero);
        testInstance.Next(zero, zero).Assert().Is(zero);

        TValueType sample = testInstance.Next<TValueType>();
        Limiter.Hundred.StallUntil(
            "Variance testing.",
            () => !sample.Equals(testInstance.Next<TValueType>()),
            TestContext.Current.CancellationToken
        );

        Limiter.Myriad.Repeat(
            "Random max testing.",
            () =>
            {
                TValueType sample = testInstance.Next<TValueType>();
                if (sample.CompareTo(zero) > 0)
                {
                    TestNext(sample);
                }
            },
            TestContext.Current.CancellationToken
        );
    }

    private void TestNextRange<TValueType>(
        TValueType min,
        TValueType max,
        Func<TValueType, TValueType> addSome,
        Func<TValueType, TValueType> subtractSome
    )
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        TestNext(max);
        TestNext(min, max);

        min.Assert(x => testInstance.Next(max, x)).Throws<ArgumentOutOfRangeException>();

        Limiter.Myriad.Repeat(
            "Minimal range adherence testing.",
            () =>
            {
                TestNext(min, addSome(min));
                TestNext(subtractSome(max), max);

                TValueType sample = testInstance.Next(addSome(min), subtractSome(max));
                TestNext(subtractSome(sample), sample);
                TestNext(sample, sample);
                TestNext(sample, addSome(sample));
            },
            TestContext.Current.CancellationToken
        );

        Limiter.Myriad.Repeat(
            "Random range testing.",
            () =>
            {
                TValueType sample1 = testInstance.Next<TValueType>();
                TValueType sample2 = testInstance.Next<TValueType>();
                if (sample1.CompareTo(sample2) < 0)
                {
                    TestNext(sample1, sample2);
                }
                else if (sample1.CompareTo(sample2) > 0)
                {
                    TestNext(sample2, sample1);
                }
            },
            TestContext.Current.CancellationToken
        );
    }

    private void TestNextOverflow<TValueType>(TValueType halfMin, TValueType halfMax)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        Limiter.Myriad.Repeat(
            "Potential overflow testing.",
            () => TestNext(halfMin, halfMax),
            TestContext.Current.CancellationToken
        );
    }

    private void TestNext<TValueType>(TValueType max)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        TValueType min = default;
        testInstance.Next(max).Assert().GreaterThanOrEqualTo(min).And.LessThan(max);
    }

    private void TestNext<TValueType>(TValueType min, TValueType max)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        testInstance.Next(min, max).Assert().GreaterThanOrEqualTo(min).And.LessThanOrEqualTo(max);
    }

    [Theory, RandomData]
    public void Next_UnsupportedTypeThrows(StructSample sample)
    {
        testInstance.Assert(x => x.Next<StructSample>()).Throws<UnsupportedException>();
        testInstance.Assert(x => x.Next(typeof(StructSample))).Throws<UnsupportedException>();
        testInstance.Assert(x => x.Next(sample)).Throws<UnsupportedException>();
        testInstance.Assert(x => x.Next(sample, sample)).Throws<UnsupportedException>();
    }

    [Fact]
    public void Next_SignedMinValueIncluded()
    {
        const sbyte min = sbyte.MinValue / 2 - 1;
        const sbyte max = sbyte.MaxValue / 2 + 1;

        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => testInstance.Next(max) == default,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding min value.",
            () => testInstance.Next(min, max) == min,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public void Next_UnsignedMinValueIncluded()
    {
        const byte min = byte.MaxValue / 4;
        const byte max = byte.MaxValue / 4 * 3;

        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => testInstance.Next(max) == default,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding min value.",
            () => testInstance.Next(min, max) == min,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public void Next_SignedMaxValueIncludedOnlyWithMin()
    {
        const sbyte min = sbyte.MinValue / 2 - 1;
        const sbyte max = sbyte.MaxValue / 2 + 1;

        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => testInstance.Next(max) != max,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => testInstance.Next(min, max) == max,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public void Next_UnsignedMaxValueIncludedOnlyWithMin()
    {
        const byte min = byte.MaxValue / 4;
        const byte max = byte.MaxValue / 4 * 3;

        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => testInstance.Next(max) != max,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => testInstance.Next(min, max) == max,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public void Next_HandlesDoubleSpecialValues()
    {
        TestNext(double.NegativeInfinity, double.NegativeInfinity);
        TestNext(double.PositiveInfinity, double.PositiveInfinity);

        testInstance.Next(0, double.PositiveInfinity).Assert().LessThanOrEqualTo(double.MaxValue);
        testInstance
            .Next(double.NegativeInfinity, 0)
            .Assert()
            .GreaterThanOrEqualTo(double.MinValue);
        testInstance
            .Next(double.NegativeInfinity, double.PositiveInfinity)
            .Assert()
            .GreaterThanOrEqualTo(double.MinValue)
            .And.LessThanOrEqualTo(double.MaxValue);

        testInstance.Next(double.NaN, 0).Assert().Is(double.NaN);
        testInstance.Next(double.NaN, double.NaN).Assert().Is(double.NaN);
    }

    [Fact]
    public void Next_HandlesFloatSpecialValues()
    {
        TestNext(float.NegativeInfinity, float.NegativeInfinity);
        TestNext(float.PositiveInfinity, float.PositiveInfinity);

        testInstance.Next(0, float.PositiveInfinity).Assert().LessThanOrEqualTo(float.MaxValue);
        testInstance.Next(float.NegativeInfinity, 0).Assert().GreaterThanOrEqualTo(float.MinValue);
        testInstance
            .Next(float.NegativeInfinity, float.PositiveInfinity)
            .Assert()
            .GreaterThanOrEqualTo(float.MinValue)
            .And.LessThanOrEqualTo(float.MaxValue);

        testInstance.Next(float.NaN, 0).Assert().Is(float.NaN);
        testInstance.Next(float.NaN, float.NaN).Assert().Is(float.NaN);
    }

    [Fact]
    public void Next_MinMustBeSmallerThanMax()
    {
        testInstance.Assert(x => x.Next(1, -1)).Throws<ArgumentOutOfRangeException>();
    }

    [Theory, RandomData]
    public void NextItem_CollectionsWork(ICollection<string> data)
    {
        data.Assert().Contains(testInstance.NextItem(data));
    }

    [Theory, RandomData]
    public void NextItem_ReadOnlyCollectionsWork(IReadOnlyCollection<string> data)
    {
        data.Assert().Contains(testInstance.NextItem(data));
    }

    [Fact]
    public void NextItem_YieldWorks()
    {
        testInstance.NextItem(CreateSeries(1)).Assert().IsNotNull();
        testInstance.NextItem(CreateSeries(2)).Assert().IsNotNull();
        testInstance.NextItem(CreateSeries(3)).Assert().IsNotNull();
    }

    [Fact]
    public void NextItem_EmptyThrows()
    {
        testInstance.Assert(x => x.NextItem(CreateSeries(0))).Throws<InvalidOperationException>();
        testInstance
            .Assert(x => x.NextItem(Array.Empty<object>()))
            .Throws<InvalidOperationException>();
    }

    [Theory, RandomData]
    public void NextItemOrDefault_SingleValueGivesIt(int value, object item)
    {
        testInstance.NextItemOrDefault([value]).Assert().Is(value);
        testInstance.NextItemOrDefault(CreateSeries(1)).Assert().IsNotNull();
        testInstance.NextItemOrDefault(new Queue<object>([item])).Assert().Is(item);
    }

    [Fact]
    public void NextItemOrDefault_EmptyGivesDefault()
    {
        testInstance.NextItemOrDefault((int[])null).Assert().Is(0);
        testInstance.NextItemOrDefault(CreateSeries(0)).Assert().IsNull();
        testInstance.NextItemOrDefault((object[])null).Assert().IsNull();
        testInstance.NextItemOrDefault(Array.Empty<object>()).Assert().IsNull();
    }

    [Theory, RandomData]
    public void NextSequence_ShufflesValues([Size(3)] int[] items)
    {
        Limiter.Hundred.StallUntil(
            "Testing shuffle randomization.",
            () => testInstance.NextSequence(items).First() == items[0],
            TestContext.Current.CancellationToken
        );
        Limiter.Hundred.StallUntil(
            "Testing shuffle randomization.",
            () => testInstance.NextSequence(items).First() == items[2],
            TestContext.Current.CancellationToken
        );
    }

    private static IEnumerable<object> CreateSeries(int size)
    {
        for (int i = 0; i < size; i++)
        {
            yield return new object();
        }
    }
}
