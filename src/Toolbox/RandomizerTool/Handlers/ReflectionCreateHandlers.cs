using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

internal static class ReflectionCreateHandlers
{
    /// <summary>Ignored due being performance heavy to call under certain circumstances.</summary>
    private static readonly ImmutableHashSet<string> _MethodsToExclude =
    [
        "Parse",
        "Format",
        "Compare",
        "ToString",
    ];

    /// <summary>Potential types to randomize.</summary>
    internal static FrozenSet<Type> PossibleTypes { get; } =
    [
        typeof(int),
        typeof(Guid),
        typeof(long),
        typeof(long?),
        typeof(int[]),
        typeof(double),
        typeof(object),
        typeof(List<double>),
        typeof(ISet<string>),
        typeof(AggregateException),
        typeof(IEnumerable<string>),
        typeof(KeyValuePair<int, string>),
        typeof(InvalidOperationException),
        typeof(ValueTuple<Guid, long, string>),
    ];

    /// <summary>Potential constructors to randomize.</summary>
    internal static FrozenSet<ConstructorInfo> PossibleConstructors { get; } =
        PossibleTypes.SelectMany(t => TypeDescriber.For(t).Constructors.OnlyPublic).ToFrozenSet();

    /// <summary>Potential methods to randomize.</summary>
    internal static FrozenSet<MethodInfo> PossibleMethods { get; } =
        PossibleTypes
            .SelectMany(t => t.GetMethods())
            .Where(m => m.GetParameters().All(p => !p.ParameterType.IsByRef))
            .Where(m => !m.ReturnType.Inherits(typeof(ValueTuple<,>)))
            .Where(m => !m.IsGenericMethodDefinition)
            .Where(m => !_MethodsToExclude.Contains(m.Name))
            .ToFrozenSet();

    /// <summary>Potential properties to randomize.</summary>
    internal static FrozenSet<PropertyInfo> PossibleProperties { get; } =
        PossibleTypes.SelectMany(t => t.GetProperties()).ToFrozenSet();

    /// <summary>Potential fields to randomize.</summary>
    internal static FrozenSet<FieldInfo> PossibleFields { get; } =
        PossibleTypes.SelectMany(t => t.GetFields()).ToFrozenSet();

    /// <summary>Potential constants to randomize.</summary>
    internal static FrozenSet<FieldInfo> PossibleConstants { get; } =
        PossibleTypes
            .SelectMany(t => t.GetFields())
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .ToFrozenSet();

    /// <summary>Potential parameters to randomize.</summary>
    internal static FrozenSet<ParameterInfo> PossibleParameters { get; } =
        PossibleTypes
            .SelectMany(t => t.GetMethods())
            .SelectMany(m => m.GetParameters())
            .ToFrozenSet();

    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
    [
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeType,
            rand => rand.Options.Gen.NextItem(PossibleTypes)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeConstructorInfoType,
            rand => rand.Options.Gen.NextItem(PossibleConstructors)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeMethodInfoType,
            rand => rand.Options.Gen.NextItem(PossibleMethods)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimePropertyInfoType,
            rand => rand.Options.Gen.NextItem(PossibleProperties)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RtFieldInfoType,
            rand => rand.Options.Gen.NextItem(PossibleFields)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.MdFieldInfoType,
            rand => rand.Options.Gen.NextItem(PossibleConstants)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeParameterInfoType,
            rand => rand.Options.Gen.NextItem(PossibleParameters)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeAssemblyType,
            rand =>
                rand.Options.Gen.NextItem(
                    AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic)
                )
        ),
        new FactoryCreateHandler<AssemblyName>(rand => rand.Create<Assembly>().GetName()),
    ];
}
