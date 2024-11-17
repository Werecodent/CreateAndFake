using System.Reflection;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFake.Toolbox.FakerTool;

/// <inheritdoc cref="IFaker"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public sealed class Faker(FakerOptions options) : IFaker
{
    /// <inheritdoc/>
    public FakerOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public bool Supports<T>()
    {
        return Supports<T>(null);
    }

    /// <inheritdoc/>
    public bool Supports<T>(FakerMod? optionConfiguration)
    {
        return Subclasser.Supports<T>();
    }

    /// <inheritdoc/>
    public bool Supports(Type type)
    {
        return Supports(type, null);
    }

    /// <inheritdoc/>
    public bool Supports(Type type, FakerMod? optionConfiguration)
    {
        return Subclasser.Supports(type);
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(params IEnumerable<Type> interfaces)
    {
        return Mock<T>(null, interfaces);
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(FakerMod? optionConfiguration, params IEnumerable<Type> interfaces)
    {
        IFaked provider = Subclasser.Create(typeof(T), interfaces);
        provider.FakeMeta.Options = optionConfiguration?.Invoke(Options) ?? Options;
        return new Fake<T>(provider);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, params IEnumerable<Type> interfaces)
    {
        return Mock(parent, null, interfaces);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, FakerMod? optionConfiguration, params IEnumerable<Type> interfaces)
    {
        IFaked provider = Subclasser.Create(parent, interfaces);
        provider.FakeMeta.Options = optionConfiguration?.Invoke(Options) ?? Options;
        return new Fake(provider);
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(params IEnumerable<Type> interfaces)
    {
        return Stub<T>(null, interfaces);
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(FakerMod? optionConfiguration, params IEnumerable<Type> interfaces)
    {
        Fake<T> fake = Mock<T>(interfaces);
        fake.ThrowByDefault = false;
        return fake;
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, params IEnumerable<Type> interfaces)
    {
        return Stub(parent, null, interfaces);
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, FakerMod? optionConfiguration, params IEnumerable<Type> interfaces)
    {
        Fake fake = Mock(parent, interfaces);
        fake.ThrowByDefault = false;
        return fake;
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(params IEnumerable<object> values)
    {
        return InjectMocks<T>(null, values);
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(FakerMod? optionConfiguration, params IEnumerable<object> values)
    {
        return Inject<T>(values?.ToArray() ?? [], (Type t) => Mock(t));
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(params IEnumerable<object> values)
    {
        return InjectStubs<T>(null, values);
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(FakerMod? optionConfiguration, params IEnumerable<object> values)
    {
        return Inject<T>(values?.ToArray() ?? [], (Type t) => Stub(t));
    }

    /// <summary>Creates an instance injected with fakes.</summary>
    /// <typeparam name="T">Instance to be created.</typeparam>
    /// <param name="values">Values to inject instead where possible.</param>
    /// <param name="subclasser">Fake creation method to use.</param>
    /// <returns>The created instance with its fakes.</returns>
    private Injected<T> Inject<T>(ICollection<object> values, Func<Type, Fake> subclasser)
    {
        Type[] startingTypes = values
            .Where(v => v != null)
            .Select(v => (v is Fake fake) ? fake.Dummy : v)
            .Select(v => v.GetType())
            .ToArray();

        ConstructorInfo? maker = FindBestConstructor<T>(startingTypes);
        if (maker != null)
        {
            object?[] args = CreateInjectArgs(maker, values, subclasser);

            return new Injected<T>((T)maker.Invoke(args
                .Select(v => (v is Fake fake) ? fake.Dummy : v)
                .ToArray()), args.OfType<Fake>());
        }
        else
        {
            throw new InvalidOperationException($"No constructors found on type '{typeof(T).Name}'.");
        }
    }

    /// <summary>Creates the args to inject an instance with.</summary>
    /// <param name="maker">Constructor to use.</param>
    ///  <param name="values">Values to inject instead where possible.</param>
    /// <param name="subclasser">Fake creation method to use.</param>
    /// <returns>The created args to inject an instance with.</returns>
    private object?[] CreateInjectArgs(ConstructorInfo maker, IEnumerable<object> values, Func<Type, Fake> subclasser)
    {
        List<Tuple<Type, object>> data = values
            .Where(v => v != null)
            .Select(v => Tuple.Create((v is Fake fake) ? fake.Dummy.GetType() : v.GetType(), v))
            .ToList();

        ParameterInfo[] info = maker.GetParameters();
        object?[] args = new object[info.Length];

        for (int i = 0; i < args.Length; i++)
        {
            Tuple<Type, object>? match = data.FirstOrDefault(t => t.Item1.Inherits(info[i].ParameterType));
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
        return typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            .GroupBy(c => c.GetParameters().Count(p => startingTypes.Any(t => t.Inherits(p.ParameterType))))
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()
            ?.OrderByDescending(c => c.GetParameters())
            .FirstOrDefault();
    }
}
