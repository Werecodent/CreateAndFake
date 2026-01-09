using System.Collections.Frozen;
using System.Globalization;
using System.Reflection;
using System.Text;
using CreateAndFake.Design;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Engine;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing common instances for <see cref="IRandomizer"/>.</summary>
public sealed class CommonSystemCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly FrozenDictionary<Type, Func<IRandomizerChainer, object>> _Gens =
        new Dictionary<Type, Func<IRandomizerChainer, object>>()
        {
            {
                typeof(CultureInfo),
                rand => rand.Options.Gen.NextItem(CultureInfo.GetCultures(CultureTypes.AllCultures))
            },
            { typeof(DateTimeFormatInfo), rand => rand.Create<CultureInfo>().DateTimeFormat },
            { typeof(NumberFormatInfo), rand => rand.Create<CultureInfo>().NumberFormat },
            { typeof(TimeSpan), rand => new TimeSpan(rand.Options.Gen.Next<long>()) },
            {
                typeof(DateTime),
                rand => new DateTime(
                    rand.Options.Gen.Next(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks),
                    DateTimeKind.Utc
                )
            },
            { typeof(DateTimeOffset), rand => new DateTimeOffset(rand.Create<DateTime>()) },
            {
                typeof(Guid),
                rand => new Guid([.. Enumerable.Range(0, 16).Select(_ => rand.Create<byte>())])
            },
            { typeof(IntPtr), rand => new IntPtr(rand.Options.Gen.Next<int>()) },
            {
                typeof(Assembly),
                rand => rand.Options.Gen.NextItem(AppDomain.CurrentDomain.GetAssemblies())
            },
            {
                typeof(IConfigurationSection),
                rand => rand.Create<Fake<IConfigurationSection>>().Dummy
            },
            { typeof(AssemblyName), rand => rand.Create<Assembly>().GetName() },
            { typeof(Uri), rand => rand.Create<UriBuilder>().Uri },
            {
                typeof(UriBuilder),
                rand => new UriBuilder(
                    rand.Create<bool>() ? "http" : "https",
                    rand.Create<string>(),
                    rand.Options.Gen.Next(-1, 65535)
                )
            },
            { typeof(StringBuilder), rand => new StringBuilder(rand.Create<string>()) },
            { typeof(CancellationToken), _ => new CancellationToken(false) },
        }.ToFrozenDictionary();

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type != null && _Gens.TryGetValue(type, out Func<IRandomizerChainer, object?>? gen))
        {
            return new(gen.Invoke(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }
}
