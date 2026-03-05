using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using CreateAndFake.RandomizerTool.Engine;
using static System.TimeZoneInfo;

namespace CreateAndFake.RandomizerTool.Handlers;

internal static class SystemCreateHandlers
{
    private static readonly ImmutableArray<CultureInfo> _PossibleCultureInfos =
    [
        .. CultureInfo.GetCultures(CultureTypes.AllCultures),
    ];

    private static readonly ImmutableArray<TimeZoneInfo> _PossibleTimeZoneInfos =
    [
        .. GetSystemTimeZones(),
    ];

    private static readonly ImmutableArray<TransitionTime> _PossibleTransitionTimes =
    [
        .. _PossibleTimeZoneInfos
            .SelectMany(d => d.GetAdjustmentRules())
            .SelectMany(d => new[] { d.DaylightTransitionStart, d.DaylightTransitionEnd }),
    ];

    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
    [
        new FactoryCreateHandler<IntPtr>(rand => new IntPtr(rand.Options.Gen.Next<int>())),
        new FactoryCreateHandler<UIntPtr>(rand => new UIntPtr(rand.Options.Gen.Next<uint>())),
        new FactoryCreateHandler<Uri>(rand => rand.Create<UriBuilder>().Uri),
        new FactoryCreateHandler<Guid>(rand => new Guid(rand.Options.Gen.NextBytes(16))),
        new FactoryCreateHandler<StringBuilder>(rand => new StringBuilder(rand.Create<string>())),
        new FactoryCreateHandler<NumberFormatInfo>(rand => rand.Create<CultureInfo>().NumberFormat),
        new FactoryCreateHandler<TimeZoneInfo>(rand =>
            rand.Options.Gen.NextItem(_PossibleTimeZoneInfos)
        ),
        new FactoryCreateHandler<TransitionTime>(rand =>
            rand.Options.Gen.NextItem(_PossibleTransitionTimes)
        ),
        new FactoryCreateHandler<CancellationToken>(rand => new CancellationToken(
            rand.Options.Gen.Next<bool>()
        )),
        new FactoryCreateHandler<CultureInfo>(rand =>
            rand.Options.Gen.NextItem(_PossibleCultureInfos)
        ),
        new FactoryCreateHandler<DateTimeFormatInfo>(rand =>
            rand.Create<CultureInfo>().DateTimeFormat
        ),
        new FactoryCreateHandler<DateTimeOffset>(rand => new DateTimeOffset(
            rand.Create<DateTime>()
        )),
        new FactoryCreateHandler<UriBuilder>(rand => new UriBuilder(
            rand.Create<bool>() ? "http" : "https",
            rand.Create<string>(),
            rand.Options.Gen.Next(-1, 65535)
        )),
    ];
}
