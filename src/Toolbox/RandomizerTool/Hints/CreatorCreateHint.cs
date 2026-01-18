using System.Globalization;
using System.Reflection;
using System.Text;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool.Engine;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="ICreator"/> supported types for <see cref="IRandomizer"/>.</summary>
public sealed class CreatorCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ICreator[] _Creators =
    [
        new Creator<AssemblyName>(rand => rand.Create<Assembly>().GetName()),
        new Creator<CancellationToken>(_ => new CancellationToken(false)),
        new Creator<DateTimeFormatInfo>(rand => rand.Create<CultureInfo>().DateTimeFormat),
        new Creator<DateTimeOffset>(rand => new DateTimeOffset(rand.Create<DateTime>())),
        new Creator<IFaked>(rand => (IFaked)rand.Options.Faker.Stub<object>().Dummy),
        new Creator<IntPtr>(rand => new IntPtr(rand.Options.Gen.Next<int>())),
        new Creator<NumberFormatInfo>(rand => rand.Create<CultureInfo>().NumberFormat),
        new Creator<StringBuilder>(rand => new StringBuilder(rand.Create<string>())),
        new Creator<TimeSpan>(rand => new TimeSpan(rand.Options.Gen.Next<long>())),
        new Creator<Uri>(rand => rand.Create<UriBuilder>().Uri),
        new Creator<Assembly>(rand =>
            rand.Options.Gen.NextItem(AppDomain.CurrentDomain.GetAssemblies())
        ),
        new Creator<CultureInfo>(rand =>
            rand.Options.Gen.NextItem(CultureInfo.GetCultures(CultureTypes.AllCultures))
        ),
        new Creator<DateTime>(rand => new DateTime(
            rand.Options.Gen.Next(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks),
            DateTimeKind.Utc
        )),
        new Creator<Guid>(rand => new Guid([
            .. Enumerable.Range(0, 16).Select(_ => rand.Create<byte>()),
        ])),
        new Creator<IConfigurationSection>(rand =>
            rand.Create<Fake<IConfigurationSection>>().Dummy
        ),
        new Creator<RandomizerOptions>(rand =>
            rand.Options with
            {
                Gen = rand.Create<SeededRandom>(),
                CollectionMinSize = rand.Options.Gen.Next(0, 2),
                CollectionMaxSize = rand.Options.Gen.Next(0, 5),
                StringMinSize = rand.Options.Gen.Next(0, 4),
                StringMaxSize = rand.Options.Gen.Next(0, 10),
            }
        ),
        new Creator<UriBuilder>(rand => new UriBuilder(
            rand.Create<bool>() ? "http" : "https",
            rand.Create<string>(),
            rand.Options.Gen.Next(-1, 65535)
        )),
    ];

    private static readonly IDictionary<Type, ICreator[]> _CreatorsByType =
        TypeSupporter.GroupByInheritance(_Creators);

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (
            type != null
            && type != typeof(object)
            && type != typeof(ICloneable)
            && _CreatorsByType.TryGetValue(type, out ICreator[]? creators)
        )
        {
            return new(randomizer.Options.Gen.NextItem(creators).CreateSupported(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
