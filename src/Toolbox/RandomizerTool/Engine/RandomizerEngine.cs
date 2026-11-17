using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="IRandomizer"/>
public sealed class RandomizerEngine : ToolEngine<ICreateHint>, IRandomizerEngine
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IRandomizer.Create(Type,RandomizerMod)"/>
    public object Create(Type type, IRandomizerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(type, chainer);

        CreateHintResult? result = SelectHints(chainer)
            .Select(h => h.TryToCreate(type, chainer))
            .FirstOrDefault(r => r?.HasData ?? false);

        if (result != null)
        {
            return result.Data!;
        }
        else
        {
            throw new UnsupportedException(
                $"Type '{GenericConverter.ExpandName(type)}' not supported by the randomizer. "
                    + "Create a hint to generate the type."
            );
        }
    }

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IRandomizer.Inject"/>
    public object Inject(Type type, IEnumerable<object?>? values, IRandomizerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(type);

        if (type.IsGenericTypeDefinition)
        {
            int i = 0;
            foreach (Type defined in GenericResolver.CreateConcreteGenerics(type, chainer))
            {
                ArgumentGuard.ThrowUponIterationLimit(i++, chainer.Options.Design.IterationLimit);
                try
                {
                    return HandleInject(defined, values, chainer);
                }
                catch (CreateAndFakeException)
                {
                    // Try next type.
                }
            }
            throw new UnsupportedException(
                $"Could not create generic '{GenericConverter.ExpandName(type)}'."
            );
        }
        else
        {
            return HandleInject(type, values, chainer);
        }
    }

    /// <inheritdoc cref="Inject"/>
    public object HandleInject(Type type, IEnumerable<object?>? values, IRandomizerChainer chainer)
    {
        TypeDescriber describer = TypeDescriber.For(type);

        List<Tuple<Type, object>> data =
        [
            .. (values ?? [])
                .Where(v => v != null)
                .Select(v => (v is Fake fake) ? fake.Dummy : v)
                .Where(v => v != null)
                .Select(v => Tuple.Create(v!.GetType(), v)),
        ];

        MethodBase? maker = FindBestConstructor(describer.Constructors.Visible, data)
            ?? FindBestConstructor(describer.Factories.Visible, data);

        if (
            maker == null
            || describer.Inherits<Fake>()
            || describer.Inherits(typeof(Injected<>))
            || describer.Inherits<Delegate>()
        )
        {
            return Create(type, chainer);
        }
        else if (maker is ConstructorInfo constructor)
        {
            return constructor.Invoke(CreateInjectArgs(maker, data, chainer));
        }
        else
        {
            return maker.Invoke(null, CreateInjectArgs(maker, data, chainer))!;
        }
    }

    /// <summary>Creates the args to inject an instance with.</summary>
    /// <param name="maker">Constructor to use.</param>
    /// <param name="data">Canned data to prefer.</param>
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <returns>The created args to inject an instance with.</returns>
    private object?[] CreateInjectArgs(
        MethodBase maker,
        List<Tuple<Type, object>> data,
        IRandomizerChainer chainer
    )
    {
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
            else
            {
                args[i] = Create(info[i].ParameterType, chainer);
            }
        }
        return args;
    }

    /// <summary>Finds the constructor with the most matches then by fewest parameters.</summary>
    /// <param name="potentialMakers">Potential constructors/factories to analyze.</param>
    /// <param name="data">Injection data to use.</param>
    /// <returns>Compatible constructor if found, null otherwise.</returns>
    private static MethodBase? FindBestConstructor(
        IEnumerable<MethodBase> potentialMakers,
        List<Tuple<Type, object>> data
    )
    {
        return potentialMakers
            .Where(c => !c.IsGenericMethodDefinition)
            .GroupBy(c =>
                c.GetParameters().Count(p => data.Exists(t => t.Item1.Inherits(p.ParameterType)))
            )
            .Where(g => g.Key > 0)
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()
            ?.OrderBy(c => c.GetParameters().Length)
            .FirstOrDefault();
    }
}
