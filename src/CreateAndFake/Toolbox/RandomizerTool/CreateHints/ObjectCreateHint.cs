using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing objects in general for <see cref="IRandomizer"/>.</summary>
public sealed class ObjectCreateHint : CreateHint
{
    /// <summary>Caches found subclasses for types.</summary>
    private static readonly Dictionary<Type, Type[]> _SubclassCache = new()
    {
        { typeof(object), new[] { typeof(object) } }
    };

    /// <inheritdoc/>
    protected internal override (bool, object?) TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        object? result = (type == null) ? null : Limiter
            .Dozen
            .Attempt($"Create object of type '{type}'",
                () => Create(FindTypeToCreate(type, randomizer), type, randomizer));

        return (result != null, result);
    }

    /// <param name="rootType">Original <c>Type</c> being generated.</param>
    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object? Create(Type type, Type rootType, RandomizerChainer randomizer)
    {
        if (type != rootType)
        {
            return randomizer.Create(type);
        }

        DataRandom smartData = randomizer.Options.Gen.NextData();
        object? data = CreateNew(type, randomizer, smartData);
        if (data == null)
        {
            return data;
        }

        try
        {
            Populate(data, smartData, randomizer);
        }
        catch
        {
            Disposer.Cleanup(data);
            throw;
        }

        return data;
    }

    /// <summary>Sets member data for a newly created instance.</summary>
    /// <param name="data">Instance to populate.</param>
    /// <param name="smartData">Smart data currently being utilized.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    private static void Populate(object data, DataRandom smartData, RandomizerChainer randomizer)
    {
        Type dataType = data.GetType();

        foreach (FieldInfo field in dataType.GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(f => !f.IsInitOnly && !f.IsLiteral))
        {
            string? smartValue = (field.FieldType == typeof(string))
                ? smartData.Find(field.Name)
                : null;
            field.SetValue(data, smartValue ?? randomizer.Create(field.FieldType, data));
        }
        foreach (PropertyInfo property in dataType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanWrite)
            .Where(p => p.GetSetMethod() != null))
        {
            string? smartValue = (property.PropertyType == typeof(string))
                ? smartData.Find(property.Name)
                : null;
            property.SetValue(data, smartValue ?? randomizer.Create(property.PropertyType, data));
        }
    }

    /// <summary>Creates a new instance of <paramref name="type"/>.</summary>
    /// <param name="type"><c>Type</c> to generate.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="smartData">Predefined random data.</param>
    /// <returns>The created instance.</returns>
    private static object? CreateNew(Type type, RandomizerChainer randomizer, DataRandom smartData)
    {
        /*
         * Order of preference:
         * 1) Default constructor.
         * 2) Public constructor.
         * 3) Public factory.
         * 4) Internal factory.
         * 5) Internal constructor.
         * 6) Stub.
         */

        ConstructorInfo? defaultConstructor = type.GetConstructor(Type.EmptyTypes);
        if (type == typeof(object))
        {
            IFaked fake = randomizer.Options.Faker.Stub<IFaked>().Dummy;
            fake.FakeMeta.Identifier = randomizer.Create<int>();
            return fake;
        }
        else if (defaultConstructor != null)
        {
            return defaultConstructor.Invoke(null);
        }
        else if (FindConstructors(type, BindingFlags.Public, randomizer).Any())
        {
            return CreateFrom(randomizer, smartData, (c, d) => c.Invoke(d),
                FindConstructors(type, BindingFlags.Public, randomizer));
        }
        else if (FindFactories(type, BindingFlags.Public, randomizer).Any())
        {
            return CreateFrom(randomizer, smartData, (c, d) => c.Invoke(null, d)!,
                FindFactories(type, BindingFlags.Public, randomizer));
        }
        else if (FindFactories(type, BindingFlags.NonPublic, randomizer).Any())
        {
            return CreateFrom(randomizer, smartData, (c, d) => c.Invoke(null, d)!,
                FindFactories(type, BindingFlags.NonPublic, randomizer));
        }
        else if (FindConstructors(type, BindingFlags.NonPublic, randomizer).Any())
        {
            return CreateFrom(randomizer, smartData, (c, d) => c.Invoke(d),
                FindConstructors(type, BindingFlags.NonPublic, randomizer));
        }
        else if (randomizer.Options.Faker.Supports(type))
        {
            return randomizer.Options.Faker.Stub(type).Dummy;
        }
        else
        {
            return null;
        }
    }

    /// <summary>Creates a <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T">Creation method <c>Type</c>.</typeparam>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="smartData">Predefined random data.</param>
    /// <param name="invoker">How to create the <c>Type</c> from the creation method.</param>
    /// <param name="creators">Possible creation methods.</param>
    /// <returns>The created instance.</returns>
    private static object CreateFrom<T>(RandomizerChainer randomizer, DataRandom smartData,
        Func<T, object?[], object> invoker, IEnumerable<T> creators) where T : MethodBase
    {
        T creator = randomizer.Options.Gen.NextItem(creators);

        if (creator is MethodInfo method && method.IsGenericMethodDefinition)
        {
            creator = (T)(object)method.MakeGenericMethod(method
                .GetGenericArguments()
                .Select(a => GenericCreateHint.CreateArg(a, method.ReturnType, randomizer))
                .ToArray());
        }

        return invoker.Invoke(creator, creator.GetParameters()
            .Select(p =>
            {
                string? smartValue = (p.ParameterType == typeof(string))
                    ? smartData.Find(p.Name)
                    : null;
                return smartValue ?? randomizer.Create(p.ParameterType, randomizer.Parent);
            })
            .ToArray());
    }

    /// <summary>Finds a creatable <c>Type</c> of <paramref name="type"/>.</summary>
    /// <param name="type">Parent <c>Type</c> being created.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns><c>Type</c> to use.</returns>
    private static Type FindTypeToCreate(Type type, RandomizerChainer randomizer)
    {
        Type[]? subclasses;
        lock (_SubclassCache)
        {
            if (!_SubclassCache.TryGetValue(type, out subclasses))
            {
                subclasses = FindSelfAndSubclasses(type);
                _SubclassCache.Add(type, subclasses);
            }
        }

        return randomizer.Options.Gen.NextItemOrDefault(subclasses.Where(t => !randomizer.AlreadyCreated(t))) ?? type;
    }

    /// <summary>Finds subclasses of <paramref name="type"/>.</summary>
    /// <param name="type">Parent <c>Type</c>.</param>
    /// <returns>Found subclasses.</returns>
    private static Type[] FindSelfAndSubclasses(Type type)
    {
        BindingFlags anyScope = BindingFlags.Public | BindingFlags.NonPublic;

        Type[] subclasses = type.FindLocalSubclasses()
            .Prepend(type)
            .Where(t => FindConstructors(t, anyScope).Any() || FindFactories(t, anyScope).Any())
            .Where(t => !t.IsNestedPrivate)
            .ToArray();

        if (subclasses.Length != 0)
        {
            return subclasses;
        }
        else
        {
            return type.FindLoadedSubclasses()
                .Prepend(type)
                .Where(t => FindConstructors(t, anyScope).Any() || FindFactories(t, anyScope).Any())
                .Where(t => !t.IsNestedPrivate)
                .ToArray();
        }
    }

    /// <summary>Finds <c>public</c> or <c>internal</c> constructors.</summary>
    /// <param name="type"><c>Type</c> to search for.</param>
    /// <param name="scope">Scope of constructors to look for.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Found constructors.</returns>
    private static IEnumerable<ConstructorInfo> FindConstructors(Type type,
        BindingFlags scope, RandomizerChainer? randomizer = null)
    {
        return type
            .GetConstructors(BindingFlags.Instance | scope)
            .Where(c => c.IsPublic || c.IsAssembly)
            .Where(c => randomizer == null
                || c.GetParameters().All(p => !randomizer.AlreadyCreated(p.ParameterType)));
    }

    /// <summary>Finds static methods that create <paramref name="type"/>.</summary>
    /// <param name="type"><c>Type</c> to search for.</param>
    /// <param name="scope">Scope of constructors to look for.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Found factory methods.</returns>
    private static IEnumerable<MethodInfo> FindFactories(Type type,
        BindingFlags scope, RandomizerChainer? randomizer = null)
    {
        MethodInfo[] factories = type
            .GetMethods(BindingFlags.Static | scope)
            .Where(m => m.IsPublic || m.IsAssembly)
            .Where(m => m.ReturnType.Inherits(type))
            .Where(c => randomizer == null
                || c.GetParameters().All(p => !randomizer.AlreadyCreated(p.ParameterType)))
            .ToArray();

        IEnumerable<MethodInfo> nonGenericFactories = factories.Where(m => !m.ContainsGenericParameters).ToArray();
        if (nonGenericFactories.Any())
        {
            return nonGenericFactories;
        }
        else
        {
            return factories;
        }
    }
}
