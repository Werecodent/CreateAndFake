using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Reiteration;

public sealed class LimiterTypeConverterTests : AutoTester<LimiterTypeConverter>
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt =>
            opt with
            {
                OnlyDeclaredMethods = true,
                IgnorableExceptions =
                [
                    typeof(FormatException),
                    typeof(ArgumentException),
                    typeof(InvalidCastException),
                    typeof(ArgumentOutOfRangeException),
                ],
            };

    private static readonly LimiterTypeConverter _TestInstance = new();

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
    public void ConvertTo_InvalidTypesThrow(DataSample item)
    {
        _TestInstance.Assert(x => x.ConvertTo(item, typeof(string))).Throws<ArgumentException>();
        _TestInstance.Assert(x => x.ConvertFrom(item)).Throws<ArgumentException>();
    }
}
