using System.Collections.Frozen;
using CreateAndFake.Design.Reiteration;

namespace CreateAndFake.Design.Tests.Reiteration;

public class LimiterTypeConverterTests
{
    private static readonly FrozenSet<Type> ignorableExceptions =
    [
        typeof(NotSupportedException),
        typeof(InvalidCastException),
    ];

    [Fact]
    public Task LimiterTypeConverter_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<LimiterTypeConverter>(opt =>
            opt with
            {
                IgnorableExceptions = ignorableExceptions,
            }
        );
    }

    [Fact]
    public Task LimiterTypeConverter_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<LimiterTypeConverter>(opt =>
            opt with
            {
                IgnorableExceptions = ignorableExceptions,
            }
        );
    }
}
