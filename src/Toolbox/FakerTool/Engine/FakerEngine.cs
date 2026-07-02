using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool.Engine;

/// <inheritdoc cref="IFaker"/>
public sealed class FakerEngine : ToolEngine<IFakeHint>, IFakerEngine
{
    /// <inheritdoc/>
    public bool Supports(Type type, IFakerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        return chainer.SupportedTypes.Contains(type) || Subclasser.Supports(type);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, IEnumerable<Type> interfaces, IFakerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        FakeHintResult? result = SelectHints(chainer)
            .Where(h => h.SupportsToFake)
            .Select(h => h.TryToFake(parent, interfaces, chainer))
            .FirstOrDefault(r => r?.HasData ?? false);

        IFaked instance;
        if (result != null)
        {
            instance = result.Data!;
        }
        else
        {
            throw new UnsupportedException(
                $"Type '{GenericConverter.ExpandName(parent)}' not supported by the faker. "
                    + "Create a hint to fake the type."
            );
        }

        _ = SelectHints(chainer)
            .Where(h => h.SupportsToSetup)
            .Select(h => h.TryToSetup(instance, chainer))
            .FirstOrDefault(r => r?.HasData ?? false && !r.Data);

        return new Fake(instance);
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, IEnumerable<Type> interfaces, IFakerChainer chainer)
    {
        Fake fake = Mock(parent, interfaces, chainer);
        fake.ThrowByDefault = false;
        return fake;
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(IEnumerable<object> values, IFakerChainer chainer)
    {
        return Inject<T>(values?.ToArray() ?? [], t => Mock(t, [], chainer), chainer);
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(IEnumerable<object> values, IFakerChainer chainer)
    {
        return Inject<T>(values?.ToArray() ?? [], t => Stub(t, [], chainer), chainer);
    }

    /// <summary>Creates an instance injected with fakes.</summary>
    /// <typeparam name="T">Instance to be created.</typeparam>
    /// <param name="values">Values to inject instead where possible.</param>
    /// <param name="subclasser">Fake creation method to use.</param>
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <returns>The created instance with its fakes.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private Injected<T> Inject<T>(
        ICollection<object> values,
        Func<Type, Fake> subclasser,
        IFakerChainer chainer
    )
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
            object?[] args = CreateInjectArgs(maker, values, subclasser, chainer);

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
    /// <param name="values">Values to inject instead where possible.</param>
    /// <param name="subclasser">Fake creation method to use.</param>
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <returns>The created args to inject an instance with.</returns>
    private object?[] CreateInjectArgs(
        ConstructorInfo maker,
        IEnumerable<object> values,
        Func<Type, Fake> subclasser,
        IFakerChainer chainer
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
            else if (Supports(info[i].ParameterType, chainer))
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
        return TypeDescriber
            .For<T>()
            .Constructors.OnlyPublic.GroupBy(c =>
                c.GetParameters().Count(p => startingTypes.Any(t => t.Inherits(p.ParameterType)))
            )
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()
            ?.OrderByDescending(c => c.GetParameters())
            .FirstOrDefault();
    }
}
