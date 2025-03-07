using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.RandomizerTool.CreateHints;

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
    private static readonly FrozenDictionary<Type, Func<RandomizerChainer, object>> _Gens =
        new Dictionary<Type, Func<RandomizerChainer, object>>()
        {
            { typeof(Type).GetType(), rand => rand.Create<Type>() },
            { typeof(Type), rand => rand.Options.Gen.NextItem(_PossibleTypes) },
            { typeof(MemberInfo), rand => rand.Create<MethodBase>() },
            {
                typeof(ConstructorInfo),
                rand => FindTypeInfo(rand, t => t.GetConstructors().Where(c => c.IsPublic))
            },
            { typeof(PropertyInfo), rand => FindTypeInfo(rand, t => t.GetProperties()) },
            {
                typeof(MethodInfo),
                rand => FindTypeInfo(rand, t => t.GetMethods().Where(m => m.IsPublic))
            },
            {
                typeof(FieldInfo),
                rand => FindTypeInfo(rand, t => t.GetFields().Where(f => f.IsPublic))
            },
            {
                typeof(ParameterInfo),
                rand => FindTypeInfo(rand, t => t.GetMethods().SelectMany(m => m.GetParameters()))
            },
            {
                typeof(MethodBase),
                rand =>
                    FindTypeInfo(
                        rand,
                        t =>
                            t.GetConstructors()
                                .Cast<MethodBase>()
                                .Concat(t.GetMethods())
                                .Where(m => m.IsPublic)
                    )
            },
        }.ToFrozenDictionary();

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type != null && _Gens.TryGetValue(type, out Func<RandomizerChainer, object?>? gen))
        {
            return new(gen.Invoke(randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <summary>Finds a random member info.</summary>
    /// <typeparam name="T"><c>Type</c> being found.</typeparam>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="grabber">How members are found on a <c>Type</c>.</param>
    /// <returns>The found member.</returns>
    private static T FindTypeInfo<T>(
        RandomizerChainer randomizer,
        Func<Type, IEnumerable<T>> grabber
    )
    {
        T[] result;
        do
        {
            Type foundType = (Type)_Gens[typeof(Type)].Invoke(randomizer);
            result = foundType.IsPublic ? [.. grabber.Invoke(foundType)] : [];
        } while (result.Length == 0);

        return randomizer.Options.Gen.NextItem(result);
    }
}
