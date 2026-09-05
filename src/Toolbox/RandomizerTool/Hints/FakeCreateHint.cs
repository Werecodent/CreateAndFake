using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Hints;

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

        Lock resultLock = new();
        Dictionary<Tuple<string, Type>, object> resultCache = [];
        DataRandom smartData = randomizer.Options.Gen.NextData();

        Fake mock = randomizer.Options.Faker.Stub(
            target,
            [],
            opt =>
                opt with
                {
                    FakeDefaultGenerator = method =>
                    {
                        ArgumentGuard.ThrowIfNull(method);

                        if (method.ReturnType == null)
                        {
                            return Behavior.None();
                        }

                        Tuple<string, Type> key = Tuple.Create(method.Name, method.ReturnType);
                        lock (resultLock)
                        {
                            if (!resultCache.TryGetValue(key, out object? value))
                            {
                                if (key.Item2.IsInheritedBy<string>())
                                {
                                    value =
                                        smartData.Find(key.Item1) ?? randomizer.Create<string>();
                                }
                                else
                                {
                                    value = randomizer.Create(key.Item2);
                                }
                                resultCache.Add(key, value);
                            }
                            return Behavior.Returns(value);
                        }
                    },
                }
        );

        return (Fake)type.GetConstructor([typeof(Fake)])!.Invoke([mock]);
    }
}
