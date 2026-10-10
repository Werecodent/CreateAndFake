using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using static System.TimeZoneInfo;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

#pragma warning disable CA1308 // Uri support for .NET 4.8 is lowercase.

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

    private static readonly ECCurve[] _SupportedECDsaCurves =
    [
        ECCurve.NamedCurves.nistP256,
        ECCurve.NamedCurves.nistP384,
        ECCurve.NamedCurves.nistP521,
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
        new FactoryCreateHandler<ECCurve>(rand => rand.Options.Gen.NextItem(_SupportedECDsaCurves)),
        new FactoryCreateHandler<TimeZoneInfo>(rand =>
            rand.Options.Gen.NextItem(_PossibleTimeZoneInfos)
        ),
        new FactoryCreateHandler<TransitionTime>(rand =>
            rand.Options.Gen.NextItem(_PossibleTransitionTimes)
        ),
        new FactoryCreateHandler<RuntimeMethodHandle>(rand =>
            rand.Create<MethodInfo>().MethodHandle
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
        new FactoryCreateHandler<ValueTask>(rand => new ValueTask(
            rand.Create<SingleCallValueTaskSource>(),
            rand.Create<short>()
        )),
        new FactoryCreateHandler<UriBuilder>(rand => new UriBuilder(
            rand.Create<bool>() ? "http" : "https",
            rand.Create<string>().ToLowerInvariant(),
            rand.Options.Gen.Next(-1, 65535)
        )),
    ];
}

#pragma warning restore
