using System.Globalization;
using System.Reflection;
using System.Text;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.RandomizerTool.Handlers;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="ICreateHandler"/> supported types for <see cref="IRandomizer"/>.</summary>
public sealed class HandlerCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly ICreateHandler[] _Creators =
    [
        new FactoryCreateHandler<AssemblyName>(rand => rand.Create<Assembly>().GetName()),
        new FactoryCreateHandler<CancellationToken>(_ => new CancellationToken(false)),
        new FactoryCreateHandler<DateTimeFormatInfo>(rand =>
            rand.Create<CultureInfo>().DateTimeFormat
        ),
        new FactoryCreateHandler<DateTimeOffset>(rand => new DateTimeOffset(
            rand.Create<DateTime>()
        )),
        new FactoryCreateHandler<IFaked>(rand => (IFaked)rand.Options.Faker.Stub<object>().Dummy),
        new FactoryCreateHandler<IntPtr>(rand => new IntPtr(rand.Options.Gen.Next<int>())),
        new FactoryCreateHandler<UIntPtr>(rand => new UIntPtr(rand.Options.Gen.Next<uint>())),
        new FactoryCreateHandler<NumberFormatInfo>(rand => rand.Create<CultureInfo>().NumberFormat),
        new FactoryCreateHandler<StringBuilder>(rand => new StringBuilder(rand.Create<string>())),
        new FactoryCreateHandler<Uri>(rand => rand.Create<UriBuilder>().Uri),
        new FactoryCreateHandler<Assembly>(rand =>
            rand.Options.Gen.NextItem(
                AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic)
            )
        ),
        new FactoryCreateHandler(
            AppDomain.CurrentDomain.GetAssemblies()[0].GetType(),
            rand => rand.Create<Assembly>()
        ),
        new FactoryCreateHandler<CultureInfo>(rand =>
            rand.Options.Gen.NextItem(CultureInfo.GetCultures(CultureTypes.AllCultures))
        ),
        new FactoryCreateHandler<Guid>(rand => new Guid(rand.Options.Gen.NextBytes(16))),
        new FactoryCreateHandler<IConfigurationSection>(rand =>
            rand.Create<Fake<IConfigurationSection>>().Dummy
        ),
        new FactoryCreateHandler<UriBuilder>(rand => new UriBuilder(
            rand.Create<bool>() ? "http" : "https",
            rand.Create<string>(),
            rand.Options.Gen.Next(-1, 65535)
        )),
        new StringCreateHandler(),
        new ConfigurationSectionCreateHandler(),
    ];

    private static readonly IDictionary<Type, ICreateHandler[]> _CreatorsByType =
        TypeSupporter.GroupByInheritance(
            _Creators
                .Concat(ValueCreateHandlers.Handlers)
                .Concat(ExceptionCreateHandlers.Handlers)
                .Concat(ReflectionCreateHandlers.Handlers)
                .Concat(SelfCreateHandlers.Handlers)
        );

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _CreatorsByType.Keys;

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (
            type != null
            && type != typeof(object)
            && _CreatorsByType.TryGetValue(type, out ICreateHandler[]? creators)
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
