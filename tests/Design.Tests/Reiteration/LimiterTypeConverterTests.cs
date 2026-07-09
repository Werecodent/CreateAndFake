using System.Collections.Frozen;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public class LimiterTypeConverterTests
{
    private static readonly FrozenSet<Type> _IgnorableExceptions =
    [
        typeof(FormatException),
        typeof(ArgumentException),
        typeof(InvalidCastException),
        typeof(ArgumentOutOfRangeException),
    ];

    private static readonly LimiterTypeConverter _TestInstance = new();

    [Fact]
    public Task LimiterTypeConverter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<LimiterTypeConverter>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = _IgnorableExceptions }
        );
    }

    [Fact]
    public Task LimiterTypeConverter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<LimiterTypeConverter>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = _IgnorableExceptions }
        );
    }

    [Theory, RandomData]
    public void ConvertFrom_Tries(int tries)
    {
        _TestInstance.CanConvertFrom(typeof(int)).Assert().Is(true);
        _TestInstance.ConvertFrom(Math.Abs(tries)).Assert().Is(new Limiter(Math.Abs(tries)));
    }

    [Theory, RandomData]
    public void ConvertFrom_ConvertToRoundtrip(Limiter limiter)
    {
        _TestInstance.CanConvertTo(typeof(string)).Assert().Is(true);
        _TestInstance.CanConvertFrom(typeof(string)).Assert().Is(true);

        string result = (string)_TestInstance.ConvertTo(limiter, typeof(string));

        _TestInstance.ConvertFrom(result).Assert().Is(limiter);
    }

    [Theory, RandomData]
    public void ConvertTo_InvalidTypesThrow(object item)
    {
        _TestInstance.Assert(x => x.ConvertTo(item, typeof(string))).Throws<ArgumentException>();
        _TestInstance.Assert(x => x.ConvertFrom(item)).Throws<ArgumentException>();
    }
}
