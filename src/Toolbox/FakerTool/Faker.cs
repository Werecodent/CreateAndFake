using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool;

/// <inheritdoc cref="IFaker"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Faker(FakerOptions options) : IFaker
{
    /// <inheritdoc/>
    public FakerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public bool Supports<T>(FakerMod? optionConfiguration = null)
    {
        return Subclasser.Supports<T>();
    }

    /// <inheritdoc/>
    public bool Supports(Type type, FakerMod? optionConfiguration = null)
    {
        return Subclasser.Supports(type);
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(params IEnumerable<Type> interfaces)
    {
        return Mock<T>(interfaces, null);
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        IFaked provider = Subclasser.Create(typeof(T), options, interfaces);
        provider.FakeMeta.Options = optionConfiguration?.Invoke(Options) ?? Options;
        return new Fake<T>(provider);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, params IEnumerable<Type> interfaces)
    {
        return Mock(parent, interfaces, null);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        IFaked provider = Subclasser.Create(parent, options, interfaces);
        provider.FakeMeta.Options = optionConfiguration?.Invoke(Options) ?? Options;
        return new Fake(provider);
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(params IEnumerable<Type> interfaces)
    {
        return Stub<T>(interfaces, null);
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        Fake<T> fake = Mock<T>(interfaces);
        fake.ThrowByDefault = false;
        return fake;
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, params IEnumerable<Type> interfaces)
    {
        return Stub(parent, interfaces, null);
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        Fake fake = Mock(parent, interfaces);
        fake.ThrowByDefault = false;
        return fake;
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(FakerMod? optionConfiguration = null)
    {
        return InjectMocks<T>([], optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(
        IEnumerable<object> values,
        FakerMod? optionConfiguration = null
    )
    {
        return Inject<T>(values?.ToArray() ?? [], t => Mock(t));
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(FakerMod? optionConfiguration = null)
    {
        return InjectStubs<T>([], optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(
        IEnumerable<object> values,
        FakerMod? optionConfiguration = null
    )
    {
        return Inject<T>(values?.ToArray() ?? [], t => Stub(t));
    }

    /// <summary>Creates an instance injected with fakes.</summary>
    /// <typeparam name="T">Instance to be created.</typeparam>
    /// <param name="values">Values to inject instead where possible.</param>
    /// <param name="subclasser">Fake creation method to use.</param>
    /// <returns>The created instance with its fakes.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private Injected<T> Inject<T>(ICollection<object> values, Func<Type, Fake> subclasser)
    {
        Type[] startingTypes =
        [
            .. values
                .Where(v => v != null)
                .Select(v => (v is Fake fake) ? fake.Dummy : v)
                .Select(v => v.GetType()),
        ];

        ConstructorInfo? maker = FindBestConstructor<T>(startingTypes);
        if (maker != null)
        {
            object?[] args = CreateInjectArgs(maker, values, subclasser);

            return new Injected<T>(
                (T)maker.Invoke([.. args.Select(v => (v is Fake fake) ? fake.Dummy : v)]),
                args.OfType<Fake>()
            );
        }
        else
        {
            throw new InvalidOperationException(
                $"No constructors found on type '{typeof(T).Name}'."
            );
        }
    }

    /// <summary>Creates the args to inject an instance with.</summary>
    /// <param name="maker">Constructor to use.</param>
    ///  <param name="values">Values to inject instead where possible.</param>
    /// <param name="subclasser">Fake creation method to use.</param>
    /// <returns>The created args to inject an instance with.</returns>
    private object?[] CreateInjectArgs(
        ConstructorInfo maker,
        IEnumerable<object> values,
        Func<Type, Fake> subclasser
    )
    {
        List<Tuple<Type, object>> data =
        [
            .. values
                .Where(v => v != null)
                .Select(v =>
                    Tuple.Create((v is Fake fake) ? fake.Dummy.GetType() : v.GetType(), v)
                ),
        ];

        ParameterInfo[] info = maker.GetParameters();
        object?[] args = new object[info.Length];

        for (int i = 0; i < args.Length; i++)
        {
            Tuple<Type, object>? match = data.Find(t => t.Item1.Inherits(info[i].ParameterType));
            if (match != default)
            {
                args[i] = match.Item2;
                _ = data.Remove(match);
            }
            else if (Supports(info[i].ParameterType))
            {
                args[i] = subclasser.Invoke(info[i].ParameterType);
            }
            else
            {
                args[i] = null;
            }
        }
        return args;
    }

    /// <summary>Finds the constructor with the most matches then by most parameters.</summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="startingTypes">Argument types to search on.</param>
    /// <returns>The constructor best fitted to the types.</returns>
    private static ConstructorInfo? FindBestConstructor<T>(Type[] startingTypes)
    {
        return typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            .GroupBy(c =>
                c.GetParameters().Count(p => startingTypes.Any(t => t.Inherits(p.ParameterType)))
            )
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()
            ?.OrderByDescending(c => c.GetParameters())
            .FirstOrDefault();
    }

    /// <inheritdoc/>
    public IFaker WithOptions(FakerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new Faker(optionConfiguration.Invoke(Options));
    }
}
