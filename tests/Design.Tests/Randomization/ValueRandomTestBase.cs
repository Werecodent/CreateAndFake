using System.Collections.Frozen;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Design.Tests.Randomization;

public abstract class ValueRandomTestBase<T>(T testInstance)
    where T : ValueRandom
{
    private static readonly FrozenSet<Type> ignorableExceptions =
    [
        typeof(NotSupportedException),
        typeof(ArgumentOutOfRangeException),
        typeof(EngineException),
    ];

    private readonly ValueRandom TestInstance = testInstance;

    [Fact]
    public Task ValueRandom_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<T>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = ignorableExceptions }
        );
    }

    [Fact]
    public Task ValueRandom_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<T>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = ignorableExceptions }
        );
    }

    [Fact]
    public void NextBytes_PreventsNegatives()
    {
        Tools
            .Gen.Next(short.MinValue, (short)-1)
            .Assert(TestInstance.NextBytes)
            .Throws<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Supports_InvalidTypeFalse()
    {
        TestInstance.Supports(typeof(object)).Assert().Is(false);
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

    [Fact]
    public void Supports_Bool()
    {
        TestBasicSupport<bool>(default);
        TestNext(false, true);

        bool sample = TestInstance.Next(false, true);
        Limiter.Hundred.StallUntil(
            "Variance testing.",
            () => sample != TestInstance.Next(false, true),
            TestContext.Current.CancellationToken
        );

        TestInstance.Next(false, false).Assert().Is(false);
        TestInstance.Next(true, true).Assert().Is(true);
        TestInstance.Assert(t => t.Next(true, false)).Throws<ArgumentOutOfRangeException>();
    }

    private void TestBasicSupport<TValueType>(TValueType zero)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        TestInstance.Supports<TValueType>().Assert().Is(true);
        TestInstance.Supports(typeof(TValueType)).Assert().Is(true);
        TestInstance.Next<TValueType>().Assert().IsNotNull();
        TestInstance.Next(typeof(TValueType)).Assert().IsNotNull();
        TestInstance.Next(zero).Assert().Is(zero);
        TestInstance.Next(zero, zero).Assert().Is(zero);

        TValueType sample = TestInstance.Next<TValueType>();
        Limiter.Hundred.StallUntil(
            "Variance testing.",
            () => !sample.Equals(TestInstance.Next<TValueType>()),
            TestContext.Current.CancellationToken
        );

        Limiter.Myriad.Repeat(
            "Random max testing.",
            () =>
            {
                TValueType sample = TestInstance.Next<TValueType>();
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
        TestNext(min, addSome(min));
        TestNext(subtractSome(max), max);

        min.Assert(m => TestInstance.Next(max, m)).Throws<ArgumentOutOfRangeException>();

        Limiter.Myriad.Repeat(
            "Minimal range adherence testing.",
            () =>
            {
                TValueType sample = TestInstance.Next(addSome(min), subtractSome(max));
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
                TValueType sample1 = TestInstance.Next<TValueType>();
                TValueType sample2 = TestInstance.Next<TValueType>();
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
        TestInstance.Next(max).Assert().GreaterThanOrEqualTo(min).And.LessThan(max);
    }

    private void TestNext<TValueType>(TValueType min, TValueType max)
        where TValueType : struct, IComparable, IComparable<TValueType>, IEquatable<TValueType>
    {
        TestInstance.Next(min, max).Assert().GreaterThanOrEqualTo(min).And.LessThanOrEqualTo(max);
    }

    [Theory, RandomData]
    public void Next_UnsupportedTypeThrows(StructSample sample)
    {
        TestInstance.Assert(t => t.Next<StructSample>()).Throws<NotSupportedException>();
        TestInstance.Assert(t => t.Next(typeof(StructSample))).Throws<NotSupportedException>();
        TestInstance.Assert(t => t.Next(sample)).Throws<NotSupportedException>();
        TestInstance.Assert(t => t.Next(sample, sample)).Throws<NotSupportedException>();
    }

    [Fact]
    public void Next_SignedMinValueIncluded()
    {
        const sbyte min = sbyte.MinValue / 2 - 1;
        const sbyte max = sbyte.MaxValue / 2 + 1;

        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => TestInstance.Next(max) == default,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding min value.",
            () => TestInstance.Next(min, max) == min,
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
            () => TestInstance.Next(max) == default,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding min value.",
            () => TestInstance.Next(min, max) == min,
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
            () => TestInstance.Next(max) != max,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => TestInstance.Next(min, max) == max,
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
            () => TestInstance.Next(max) != max,
            TestContext.Current.CancellationToken
        );
        Limiter.Myriad.StallUntil(
            "Finding max value.",
            () => TestInstance.Next(min, max) == max,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public void Next_HandlesDoubleSpecialValues()
    {
        TestNext(double.NegativeInfinity, double.NegativeInfinity);
        TestNext(double.PositiveInfinity, double.PositiveInfinity);

        TestInstance.Next(0, double.PositiveInfinity).Assert().LessThanOrEqualTo(double.MaxValue);
        TestInstance
            .Next(double.NegativeInfinity, 0)
            .Assert()
            .GreaterThanOrEqualTo(double.MinValue);
        TestInstance
            .Next(double.NegativeInfinity, double.PositiveInfinity)
            .Assert()
            .GreaterThanOrEqualTo(double.MinValue)
            .And.LessThanOrEqualTo(double.MaxValue);

        TestInstance.Next(double.NaN, 0).Assert().Is(double.NaN);
        TestInstance.Next(double.NaN, double.NaN).Assert().Is(double.NaN);
    }

    [Fact]
    public void Next_HandlesFloatSpecialValues()
    {
        TestNext(float.NegativeInfinity, float.NegativeInfinity);
        TestNext(float.PositiveInfinity, float.PositiveInfinity);

        TestInstance.Next(0, float.PositiveInfinity).Assert().LessThanOrEqualTo(float.MaxValue);
        TestInstance.Next(float.NegativeInfinity, 0).Assert().GreaterThanOrEqualTo(float.MinValue);
        TestInstance
            .Next(float.NegativeInfinity, float.PositiveInfinity)
            .Assert()
            .GreaterThanOrEqualTo(float.MinValue)
            .And.LessThanOrEqualTo(float.MaxValue);

        TestInstance.Next(float.NaN, 0).Assert().Is(float.NaN);
        TestInstance.Next(float.NaN, float.NaN).Assert().Is(float.NaN);
    }

    [Theory, RandomData]
    public void NextItem_CollectionsWork(ICollection<string> data)
    {
        data.Assert().Contains(TestInstance.NextItem(data));
    }

    [Theory, RandomData]
    public void NextItem_ReadOnlyCollectionsWork(IReadOnlyCollection<string> data)
    {
        data.Assert().Contains(TestInstance.NextItem(data));
    }

    [Fact]
    public void NextItem_YieldWorks()
    {
        TestInstance.NextItem(CreateSeries(1)).Assert().IsNot(null);
        TestInstance.NextItem(CreateSeries(2)).Assert().IsNot(null);
        TestInstance.NextItem(CreateSeries(3)).Assert().IsNot(null);
    }

    [Fact]
    public void NextItem_EmptyThrows()
    {
        TestInstance.Assert(t => t.NextItem(CreateSeries(0))).Throws<InvalidOperationException>();
        TestInstance
            .Assert(t => t.NextItem(Array.Empty<object>()))
            .Throws<InvalidOperationException>();
    }

    [Fact]
    public void NextItemOrDefault_EmptyGivesDefault()
    {
        TestInstance.NextItemOrDefault((int[])null).Assert().Is(0);
        TestInstance.NextItemOrDefault(CreateSeries(0)).Assert().Is(null);
        TestInstance.NextItemOrDefault((object[])null).Assert().Is(null);
        TestInstance.NextItemOrDefault(Array.Empty<object>()).Assert().Is(null);
    }

    [Theory, RandomData]
    public void NextSeries_ShufflesValues([Size(3)] int[] items)
    {
        Limiter.Hundred.StallUntil(
            "Testing shuffle randomization.",
            () => TestInstance.NextSequence(items).First() == items[0],
            TestContext.Current.CancellationToken
        );
        Limiter.Hundred.StallUntil(
            "Testing shuffle randomization.",
            () => TestInstance.NextSequence(items).First() == items[2],
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
