using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

/// <summary>Handles randomizing <see cref="Fake{T}"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class FakeCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.FakeHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [];

    /// <inheritdoc/>
    public override CreateHintResult TryToCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        if (type.Inherits(typeof(Fake<>)))
        {
            return new(Create(type, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryToCreate"/>
    private static Fake Create(Type type, IRandomizerChainer randomizer)
    {
        Type target = type.GetGenericArguments().Single();

        Dictionary<Tuple<string, Type>, object> resultCache = [];
        DataRandom smartData = randomizer.Options.Gen.NextData();

        Fake mock = randomizer.Options.Faker.Stub(
            target,
            [],
            opt =>
                opt with
                {
                    FakeDefaultGenerator = (methodName, returnType) =>
                    {
                        Tuple<string, Type> key = Tuple.Create(methodName, returnType);
                        if (!resultCache.TryGetValue(key, out object? value))
                        {
                            if (returnType.IsInheritedBy<string>())
                            {
                                value = smartData.Find(methodName) ?? randomizer.Create<string>();
                            }
                            else
                            {
                                value = randomizer.Create(returnType);
                            }
                            resultCache.Add(key, value);
                        }
                        return value;
                    },
                }
        );

        return (Fake)type.GetConstructor([typeof(Fake)])!.Invoke([mock]);
    }
}
