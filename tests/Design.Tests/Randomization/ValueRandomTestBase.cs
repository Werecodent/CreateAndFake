using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tests.TestSamples;

namespace CreateAndFake.Design.Tests.Randomization;

public abstract class ValueRandomTestBase<T>
    where T : ValueRandom
{
    private static readonly ValueRandom _TestInstance = Tools.Randomizer.Create<T>();

    [Fact]
    public Task ValueRandom_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<T>();
    }

    [Fact]
    public Task ValueRandom_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<T>();
    }

    [Fact]
    public void Supports_TypeCoverage()
    {
        _TestInstance.Supports<int>().Assert().Is(true);
        _TestInstance.Supports<uint>().Assert().Is(true);
        _TestInstance.Supports<long>().Assert().Is(true);
        _TestInstance.Supports<char>().Assert().Is(true);
        _TestInstance.Supports<byte>().Assert().Is(true);
        _TestInstance.Supports<bool>().Assert().Is(true);
        _TestInstance.Supports<ulong>().Assert().Is(true);
        _TestInstance.Supports<float>().Assert().Is(true);
        _TestInstance.Supports<short>().Assert().Is(true);
        _TestInstance.Supports<sbyte>().Assert().Is(true);
        _TestInstance.Supports<double>().Assert().Is(true);
        _TestInstance.Supports<ushort>().Assert().Is(true);
        _TestInstance.Supports<decimal>().Assert().Is(true);
    }

    [Fact]
    public void Supports_InvalidTypeFalse()
    {
        _TestInstance.Supports(typeof(object)).Assert().Is(false);
    }

    [Fact]
    public void Next_TypeCoverage()
    {
        _TestInstance.Next<int>();
        _TestInstance.Next<bool>();
        _TestInstance.Next<uint>();
        _TestInstance.Next<long>();
        _TestInstance.Next<char>();
        _TestInstance.Next<byte>();
        _TestInstance.Next<ulong>();
        _TestInstance.Next<float>();
        _TestInstance.Next<short>();
        _TestInstance.Next<sbyte>();
        _TestInstance.Next<double>();
        _TestInstance.Next<ushort>();
        _TestInstance.Next<decimal>();
    }

    [Fact]
    public void Next_MaxTypeCoverage()
    {
        _TestInstance.Next(true);
        _TestInstance.Next(int.MaxValue);
        _TestInstance.Next(byte.MaxValue);
        _TestInstance.Next(uint.MaxValue);
        _TestInstance.Next(long.MaxValue);
        _TestInstance.Next(char.MaxValue);
        _TestInstance.Next(ulong.MaxValue);
        _TestInstance.Next(float.MaxValue);
        _TestInstance.Next(short.MaxValue);
        _TestInstance.Next(sbyte.MaxValue);
        _TestInstance.Next(double.MaxValue);
        _TestInstance.Next(ushort.MaxValue);
        _TestInstance.Next(decimal.MaxValue);
    }

    [Fact]
    public void Next_MinTypeCoverage()
    {
        _TestInstance.Next(false, true);
        _TestInstance.Next(int.MinValue, int.MaxValue);
        _TestInstance.Next(byte.MinValue, byte.MaxValue);
        _TestInstance.Next(uint.MinValue, uint.MaxValue);
        _TestInstance.Next(long.MinValue, long.MaxValue);
        _TestInstance.Next(char.MinValue, char.MaxValue);
        _TestInstance.Next(ulong.MinValue, ulong.MaxValue);
        _TestInstance.Next(float.MinValue, float.MaxValue);
        _TestInstance.Next(short.MinValue, short.MaxValue);
        _TestInstance.Next(sbyte.MinValue, sbyte.MaxValue);
        _TestInstance.Next(double.MinValue, double.MaxValue);
        _TestInstance.Next(ushort.MinValue, ushort.MaxValue);
        _TestInstance.Next(decimal.MinValue, decimal.MaxValue);
    }

    [Fact]
    public void Next_StumbleWorks()
    {
        const int min = int.MinValue / 2 - 1;
        const int max = int.MaxValue / 2 + 1;

        Limiter.Myriad.Repeat(
            "Testing stumble behavior.",
            () => _TestInstance.Next(min, max).Assert().GreaterThanOrEqualTo(min).And.LessThan(max)
        );
    }

    [Fact]
    public void Next_InvalidTypeThrows()
    {
        _TestInstance.Assert(t => t.Next(typeof(object))).Throws<NotSupportedException>();
    }

    [Fact]
    public void Next_SameMinMaxWorks()
    {
        _TestInstance.Next(0, 0).Assert().Is(0);
    }

    [Fact]
    public void Next_Variation()
    {
        _TestInstance.Next<long>().Assert().IsNot(_TestInstance.Next<long>());
    }

    [Theory, RandomData]
    public void Next_UnsupportedTypeThrows(StructSample sample)
    {
        _TestInstance.Assert(t => t.Next<StructSample>()).Throws<NotSupportedException>();
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

    [Fact]
    public void Next_MinGreaterMaxThrows()
    {
        _TestInstance.Assert(t => t.Next(0, -1)).Throws<ArgumentOutOfRangeException>();
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
        _TestInstance.NextItem(CreateEnum(1)).Assert().IsNot(null);
        _TestInstance.NextItem(CreateEnum(2)).Assert().IsNot(null);
        _TestInstance.NextItem(CreateEnum(3)).Assert().IsNot(null);
    }

    [Fact]
    public void NextItem_EmptyThrows()
    {
        _TestInstance.Assert(t => t.NextItem(CreateEnum(0))).Throws<InvalidOperationException>();
        _TestInstance
            .Assert(t => t.NextItem(Array.Empty<object>()))
            .Throws<InvalidOperationException>();
    }

    [Fact]
    public void NextItemOrDefault_EmptyGivesDefault()
    {
        _TestInstance.NextItemOrDefault((int[])null).Assert().Is(0);
        _TestInstance.NextItemOrDefault(CreateEnum(0)).Assert().Is(null);
        _TestInstance.NextItemOrDefault((object[])null).Assert().Is(null);
        _TestInstance.NextItemOrDefault(Array.Empty<object>()).Assert().Is(null);
    }

    private static IEnumerable<object> CreateEnum(int size)
    {
        for (int i = 0; i < size; i++)
        {
            yield return new object();
        }
    }
}
