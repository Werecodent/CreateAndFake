using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design.Types;
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
        typeof(List<double>),
        typeof(ISet<string>),
        typeof(AggregateException),
        typeof(IEnumerable<string>),
        typeof(KeyValuePair<int, string>),
        typeof(InvalidOperationException),
        typeof(ValueTuple<Guid, long, string>),
    ];

    private static readonly FrozenSet<ConstructorInfo> _Constructors = _PossibleTypes
        .Where(t => t != typeof(string))
        .SelectMany(t => TypeDescriber.For(t).Constructors.OnlyPublic)
        .ToFrozenSet();

    private static readonly FrozenSet<MethodInfo> _Methods = _PossibleTypes
        .SelectMany(t => t.GetMethods())
        .Where(m => m.GetParameters().All(p => !p.ParameterType.IsByRef))
        .Where(m => !m.ReturnType.Inherits(typeof(ValueTuple<,>)))
        .Where(m => m.ReflectedType != typeof(string) || m.Name != nameof(string.Format))
        .ToFrozenSet();

    private static readonly FrozenSet<PropertyInfo> _Properties = _PossibleTypes
        .SelectMany(t => t.GetProperties())
        .ToFrozenSet();

    private static readonly FrozenSet<FieldInfo> _Fields = _PossibleTypes
        .SelectMany(t => t.GetFields())
        .ToFrozenSet();

    private static readonly FrozenSet<FieldInfo> _ConstFields = _PossibleTypes
        .SelectMany(t => t.GetFields())
        .Where(f => f.IsLiteral && !f.IsInitOnly)
        .ToFrozenSet();

    private static readonly FrozenSet<ParameterInfo> _Parameters = _PossibleTypes
        .SelectMany(t => t.GetMethods())
        .SelectMany(m => m.GetParameters())
        .ToFrozenSet();

    internal static IEnumerable<MethodBase> PossibleMethods =>
        Enumerable.Empty<MethodBase>().Concat(_Constructors).Concat(_Methods).Distinct();

    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
    [
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeType,
            rand => rand.Options.Gen.NextItem(_PossibleTypes)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeConstructorInfoType,
            rand => rand.Options.Gen.NextItem(_Constructors)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeMethodInfoType,
            rand => rand.Options.Gen.NextItem(_Methods)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimePropertyInfoType,
            rand => rand.Options.Gen.NextItem(_Properties)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RtFieldInfoType,
            rand => rand.Options.Gen.NextItem(_Fields)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.MdFieldInfoType,
            rand => rand.Options.Gen.NextItem(_ConstFields)
        ),
        new FactoryCreateHandler(
            RuntimeDetails.RuntimeParameterInfoType,
            rand => rand.Options.Gen.NextItem(_Parameters)
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
