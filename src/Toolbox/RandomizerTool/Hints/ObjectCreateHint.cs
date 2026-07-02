using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing objects in general for <see cref="IRandomizer"/>.</summary>
public sealed class ObjectCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.ObjectHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(object)];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        object? result =
            (type == null)
                ? null
                : randomizer.Options.ObjectCreateAttempts.Attempt(
                    $"Create object of type '{GenericTypeConverter.ExpandedName(type)}'",
                    () => Create(type, type, randomizer)
                );

        return result != null ? new(result) : CreateHintResult.None;
    }

    /// <param name="rootType">Original <see cref="Type"/> being generated.</param>
    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryToCreate"/>
    private static object? Create(Type type, Type rootType, IRandomizerChainer randomizer)
    {
        if (type != rootType)
        {
            return randomizer.Create(type);
        }

        DataRandom smartData = randomizer.Options.Gen.NextData();
        object? data = CreateNew(type, randomizer, smartData);
        if (data is null or IFaked)
        {
            return data;
        }

        bool changed = Populate(data, smartData, randomizer);
        if (changed || !randomizer.Options.ContentRandomizationRequired)
        {
            return data;
        }
        else
        {
            Disposer.Cleanup(data);
            throw new UnsupportedException(
                $"Could not randomize content for '{GenericTypeConverter.ExpandedName(data)}'."
            );
        }
    }

    /// <summary>Sets member data for a newly created instance.</summary>
    /// <param name="data">Instance to populate.</param>
    /// <param name="smartData">Smart data currently being utilized.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns><see langword="true"/> if member data existed and was changed, <see langword="false"/> otherwise.</returns>
    private static bool Populate(object data, DataRandom smartData, IRandomizerChainer randomizer)
    {
        bool canChange = false;
        bool changed = false;
        Type dataType = data.GetType();

        foreach (FieldInfo field in TypeDescriber.For(dataType).Fields.Writable)
        {
            canChange = true;

            string? smartValue =
                (field.FieldType == typeof(string)) ? smartData.Find(field.Name) : null;
            field.SetValue(data, smartValue ?? randomizer.CreateInternal(field.FieldType, data));

            changed = true;
        }
        foreach (PropertyInfo property in TypeDescriber.For(dataType).Properties.Settable)
        {
            canChange = true;

            string? smartValue =
                (property.PropertyType == typeof(string)) ? smartData.Find(property.Name) : null;

            object newValue = smartValue ?? randomizer.CreateInternal(property.PropertyType, data);

            try
            {
                property.SetValue(data, newValue);
                changed = true;
            }
            catch (Exception)
            {
                // Could not set.
            }
        }
        return canChange == changed;
    }

    /// <summary>Creates a new instance of <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to generate.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="smartData">Predefined random data.</param>
    /// <returns>The created instance.</returns>
    private static object? CreateNew(Type type, IRandomizerChainer randomizer, DataRandom smartData)
    {
        /*
         * Order of preference:
         * 1) Default constructor.
         * 2) Visible constructors.
         * 3) Visible factories.
         * 4) Stub.
         */

        ConstructorInfo? defaultConstructor = type.GetConstructor(Type.EmptyTypes);
        if (type == typeof(object))
        {
            return randomizer.Options.Faker.Stub<object>().Dummy;
        }
        else if (defaultConstructor != null)
        {
            return defaultConstructor.Invoke(null);
        }
        else if (FindConstructors(type, randomizer).Any())
        {
            return CreateFrom(
                randomizer,
                smartData,
                (c, d) => c.Invoke(d),
                FindConstructors(type, randomizer)
            );
        }
        else if (FindFactories(type, randomizer).Any())
        {
            return CreateFrom(
                randomizer,
                smartData,
                (c, d) => c.Invoke(null, d)!,
                FindFactories(type, randomizer)
            );
        }
        else if (randomizer.Options.Faker.Supports(type))
        {
            return ((Fake)randomizer.Create(typeof(Fake<>).MakeGenericType(type))).Dummy;
        }
        else
        {
            return null;
        }
    }

    /// <summary>Creates a <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T">Creation method <see cref="Type"/>.</typeparam>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="smartData">Predefined random data.</param>
    /// <param name="invoker">How to create the <see cref="Type"/> from the creation method.</param>
    /// <param name="creators">Possible creation methods.</param>
    /// <returns>The created instance.</returns>
    private static object CreateFrom<T>(
        IRandomizerChainer randomizer,
        DataRandom smartData,
        Func<T, object?[], object> invoker,
        IEnumerable<T> creators
    )
        where T : MethodBase
    {
        T creator = randomizer.Options.Gen.NextItem(creators);

        if (creator is MethodInfo method && method.IsGenericMethodDefinition)
        {
            creator = (T)(object)GenericResolver.OfConcrete(method, randomizer);
        }

        return invoker.Invoke(
            creator,
            [
                .. creator
                    .GetParameters()
                    .Select(p =>
                    {
                        string? smartValue =
                            (p.ParameterType == typeof(string)) ? smartData.Find(p.Name) : null;
                        return smartValue ?? randomizer.Create(p.ParameterType);
                    }),
            ]
        );
    }

    /// <summary>Finds <see langword="public"/> or <see langword="internal"/> constructors.</summary>
    /// <param name="type"><see cref="Type"/> to search for.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Found constructors.</returns>
    private static IEnumerable<ConstructorInfo> FindConstructors(
        Type type,
        IRandomizerChainer? randomizer
    )
    {
        return TypeDescriber
            .For(type)
            .Constructors.Visible.Where(c =>
                randomizer == null
                || c.GetParameters().All(p => !randomizer.AlreadyCreated(p.ParameterType))
            );
    }

    /// <summary>Finds static methods that create <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to search for.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <returns>Found factory methods.</returns>
    private static IEnumerable<MethodInfo> FindFactories(Type type, IRandomizerChainer? randomizer)
    {
        MethodInfo[] factories =
        [
            .. TypeDescriber
                .For(type)
                .Factories.Visible.Where(c =>
                    randomizer == null
                    || c.GetParameters().All(p => !randomizer.AlreadyCreated(p.ParameterType))
                ),
        ];

        IEnumerable<MethodInfo> nonGenericFactories =
        [
            .. factories.Where(m => !m.ContainsGenericParameters),
        ];
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
