using System.Globalization;
using System.Text;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

internal static class SystemCreateHandlers
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
    [
        new FactoryCreateHandler<CancellationToken>(_ => new CancellationToken(false)),
        new FactoryCreateHandler<IntPtr>(rand => new IntPtr(rand.Options.Gen.Next<int>())),
        new FactoryCreateHandler<UIntPtr>(rand => new UIntPtr(rand.Options.Gen.Next<uint>())),
        new FactoryCreateHandler<Uri>(rand => rand.Create<UriBuilder>().Uri),
        new FactoryCreateHandler<Guid>(rand => new Guid(rand.Options.Gen.NextBytes(16))),
        new FactoryCreateHandler<StringBuilder>(rand => new StringBuilder(rand.Create<string>())),
        new FactoryCreateHandler<NumberFormatInfo>(rand => rand.Create<CultureInfo>().NumberFormat),
        new FactoryCreateHandler<CultureInfo>(rand =>
            rand.Options.Gen.NextItem(CultureInfo.GetCultures(CultureTypes.AllCultures))
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
