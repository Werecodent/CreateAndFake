using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

internal static class ReflectionCreateHandlers
{
    /// <summary>Potential types to randomize.</summary>
    private static readonly ImmutableArray<Type> _PossibleTypes =
    [
        typeof(int),
        typeof(Guid),
        typeof(long),
        typeof(long?),
        typeof(int[]),
        typeof(double),
        typeof(string),
        typeof(object),
        typeof(DateTime),
        typeof(TimeSpan),
        typeof(List<double>),
        typeof(ISet<string>),
        typeof(AggregateException),
        typeof(IEnumerable<string>),
        typeof(KeyValuePair<int, string>),
        typeof(InvalidOperationException),
        typeof(ValueTuple<Guid, long, string>),
    ];

    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
    [
        new FactoryCreateHandler( // RuntimeType
            typeof(Type).GetType(),
            rand => rand.Options.Gen.NextItem(_PossibleTypes)
        ),
        new FactoryCreateHandler( // RuntimeConstructorInfo
            typeof(string).GetConstructors()[0].GetType(),
            rand => FindTypeInfo(rand, t => t.GetConstructors().Where(c => c.IsPublic))
        ),
        new FactoryCreateHandler( // RuntimeMethodInfo
            typeof(string).GetMethods()[0].GetType(),
            rand =>
                FindTypeInfo(
                    rand,
                    t =>
                        t.GetMethods()
                            .Where(m => m.IsPublic)
                            .Where(m => m.GetParameters().All(p => !p.ParameterType.IsByRef))
                            .Where(m => !m.ReturnType.Inherits(typeof(ValueTuple<,>)))
                )
        ),
        new FactoryCreateHandler( // RuntimePropertyInfo
            typeof(string).GetProperties()[0].GetType(),
            rand => FindTypeInfo(rand, t => t.GetProperties())
        ),
        new FactoryCreateHandler( // RuntimeFieldInfo
            typeof(string).GetFields()[0].GetType(),
            rand => FindTypeInfo(rand, t => t.GetFields().Where(f => f.IsPublic))
        ),
        new FactoryCreateHandler( // RuntimeParameterInfo
            typeof(string).GetMethods().SelectMany(m => m.GetParameters()).First().GetType(),
            rand => FindTypeInfo(rand, t => t.GetMethods().SelectMany(m => m.GetParameters()))
        ),
        new FactoryCreateHandler( // RuntimeAssembly
            AppDomain.CurrentDomain.GetAssemblies()[0].GetType(),
            rand =>
                rand.Options.Gen.NextItem(
                    AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic)
                )
        ),
        new FactoryCreateHandler<AssemblyName>(rand => rand.Create<Assembly>().GetName()),
    ];

    /// <summary>Finds a random member info.</summary>
    /// <typeparam name="T"><see cref="Type"/> being found.</typeparam>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="grabber">How members are found on a <see cref="Type"/>.</param>
    /// <returns>The found member.</returns>
    private static T FindTypeInfo<T>(
        IRandomizerChainer randomizer,
        Func<Type, IEnumerable<T>> grabber
    )
    {
        T[] result;
        do
        {
            Type foundType = randomizer.Options.Gen.NextItem(_PossibleTypes);
            result = foundType.IsPublic ? [.. grabber.Invoke(foundType)] : [];
        } while (result.Length == 0);

        return randomizer.Options.Gen.NextItem(result);
    }
}
