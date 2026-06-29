using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Tooling;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;

namespace CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="IRandomizer"/>
public sealed class RandomizerEngine : ToolEngine<CreateHint>, IRandomizerEngine
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
                $"Type '{GenericTypeConverter.ExpandedName(type)}' not supported by the randomizer. "
                    + "Create a hint to generate the type."
            );
        }
    }

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IRandomizer.Inject"/>
    public object Inject(Type type, IEnumerable<object?>? values, IRandomizerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(type);

        List<Tuple<Type, object>> data =
        [
            .. (values ?? [])
                .Where(v => v != null)
                .Select(v => (v is Fake fake) ? fake.Dummy : v)
                .Where(v => v != null)
                .Select(v => Tuple.Create(v!.GetType(), v)),
        ];

        ConstructorInfo? maker =
            FindConstructor(type, data, BindingFlags.Public)
            ?? FindConstructor(type, data, BindingFlags.NonPublic);

        if (
            maker == null
            || type.Inherits<Fake>()
            || type.Inherits(typeof(Injected<>))
            || type.Inherits<Delegate>()
        )
        {
            return Create(type, chainer);
        }
        else
        {
            return maker.Invoke(CreateInjectArgs(maker, data, chainer));
        }
    }

    /// <summary>Creates the args to inject an instance with.</summary>
    /// <param name="maker">Constructor to use.</param>
    /// <param name="data">Canned data to prefer.</param>
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <returns>The created args to inject an instance with.</returns>
    private object?[] CreateInjectArgs(
        ConstructorInfo maker,
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
    /// <param name="type">Type to find a constructor for.</param>
    /// <param name="data">Injection data to use.</param>
    /// <param name="scope">Scope of constructors to find.</param>
    /// <returns>Constructor if found, null otherwise.</returns>
    private static ConstructorInfo? FindConstructor(
        Type type,
        List<Tuple<Type, object>> data,
        BindingFlags scope
    )
    {
        return type.GetConstructors(BindingFlags.Instance | scope)
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
