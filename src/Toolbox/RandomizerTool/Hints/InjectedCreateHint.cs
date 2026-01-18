using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing injected dummies for <see cref="IRandomizer"/>.</summary>
public sealed class InjectedCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (type.Inherits(typeof(Injected<>)))
        {
            return new(Create(type, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object Create(Type type, IRandomizerChainer randomizer)
    {
        Type target = type.GetGenericArguments().Single();

        ConstructorInfo? maker =
            FindConstructor(target, randomizer, BindingFlags.Public)
            ?? FindConstructor(target, randomizer, BindingFlags.NonPublic);

        if (maker != null)
        {
            ParameterInfo[] info = maker.GetParameters();
            object?[] args = new object[info.Length];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = randomizer.Options.Faker.Supports(info[i].ParameterType)
                    ? randomizer.Create(typeof(Fake<>).MakeGenericType(info[i].ParameterType))
                    : randomizer.Create(info[i].ParameterType);
            }

            return type.GetConstructor([target, typeof(IEnumerable<Fake>)])!
                .Invoke([
                    maker.Invoke([.. args.Select(v => (v is Fake fake) ? fake.Dummy : v)]),
                    args.OfType<Fake>(),
                ]);
        }
        else
        {
            throw new InvalidOperationException($"No constructors found on type '{target}'.");
        }
    }

    /// <summary>Finds the constructor with the most class references then by fewest parameters.</summary>
    /// <param name="target"><see cref="Type"/> to find a constructor for.</param>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="scope">Scope of constructors to find.</param>
    /// <returns>Constructor if found, <see langword="null"/> otherwise.</returns>
    private static ConstructorInfo? FindConstructor(
        Type target,
        IRandomizerChainer randomizer,
        BindingFlags scope
    )
    {
        return target
            .GetConstructors(BindingFlags.Instance | scope)
            .GroupBy(c =>
                c.GetParameters().Count(p => randomizer.Options.Faker.Supports(p.ParameterType))
            )
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()
            ?.OrderBy(c => c.GetParameters())
            .FirstOrDefault();
    }
}
