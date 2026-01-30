using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.RandomizerTool.Handlers;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing type and info instances for <see cref="IRandomizer"/>.</summary>
public sealed class TypeInfoCreateHint : CreateHint
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
    private static readonly ICreateHandler[] _Creators =
    [
        new FactoryCreateHandler(
            typeof(Type).GetType(),
            rand => rand.Options.Gen.NextItem(_PossibleTypes)
        ),
        new FactoryCreateHandler<Type>(rand => rand.Options.Gen.NextItem(_PossibleTypes)),
        new FactoryCreateHandler<MemberInfo>(rand => rand.Create<MethodBase>()),
        new FactoryCreateHandler<MethodBase>(rand =>
            FindTypeInfo(
                rand,
                t =>
                    t.GetConstructors()
                        .Cast<MethodBase>()
                        .Concat(
                            t.GetMethods().Where(m => !m.ReturnType.Inherits(typeof(ValueTuple<,>)))
                        )
                        .Where(m => m.GetParameters().All(p => !p.ParameterType.IsByRef))
                        .Where(m => m.IsPublic)
            )
        ),
        new FactoryCreateHandler<ConstructorInfo>(rand =>
            FindTypeInfo(rand, t => t.GetConstructors().Where(c => c.IsPublic))
        ),
        new FactoryCreateHandler<MethodInfo>(rand =>
            FindTypeInfo(
                rand,
                t =>
                    t.GetMethods()
                        .Where(m => m.IsPublic)
                        .Where(m => m.GetParameters().All(p => !p.ParameterType.IsByRef))
                        .Where(m => !m.ReturnType.Inherits(typeof(ValueTuple<,>)))
            )
        ),
        new FactoryCreateHandler<PropertyInfo>(rand => FindTypeInfo(rand, t => t.GetProperties())),
        new FactoryCreateHandler<FieldInfo>(rand =>
            FindTypeInfo(rand, t => t.GetFields().Where(f => f.IsPublic))
        ),
        new FactoryCreateHandler<ParameterInfo>(rand =>
            FindTypeInfo(rand, t => t.GetMethods().SelectMany(m => m.GetParameters()))
        ),
        new FactoryCreateHandler(
            typeof(string).GetConstructors()[0].GetType(),
            rand => rand.Create<ConstructorInfo>()
        ),
        new FactoryCreateHandler(
            typeof(string).GetMethods()[0].GetType(),
            rand => rand.Create<MethodInfo>()
        ),
        new FactoryCreateHandler(
            typeof(string).GetProperties()[0].GetType(),
            rand => rand.Create<PropertyInfo>()
        ),
        new FactoryCreateHandler(
            typeof(string).GetFields()[0].GetType(),
            rand => rand.Create<FieldInfo>()
        ),
        new FactoryCreateHandler(
            typeof(string).GetMethods().SelectMany(m => m.GetParameters()).First().GetType(),
            rand => rand.Create<ParameterInfo>()
        ),
    ];

    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly IDictionary<Type, ICreateHandler> _CreatorsByType =
        TypeSupporter.GroupBySupportedType(_Creators);

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => _Creators.Select(c => c.SupportedType!);

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (type != null && _CreatorsByType.TryGetValue(type, out ICreateHandler? gen))
        {
            return new(gen.CreateSupported(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

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
            Type foundType = (Type)_CreatorsByType[typeof(Type)].CreateSupported(randomizer)!;
            result = foundType.IsPublic ? [.. grabber.Invoke(foundType)] : [];
        } while (result.Length == 0);

        return randomizer.Options.Gen.NextItem(result);
    }
}
