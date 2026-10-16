using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
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
        Dictionary<Tuple<MethodInfo, CallData>, Behavior> resultCache = new(
            randomizer.Options.Valuer.WithOptions(opt => opt with { SkipAsyncValues = true })
        );
        DataRandom smartData = randomizer.Options.Gen.NextData();

        Fake mock = randomizer.Options.Faker.Stub(
            target,
            [],
            opt =>
                opt with
                {
                    FakeDefaultGenerator = (method, callData) =>
                    {
                        ArgumentGuard.ThrowIfNull(method);

                        if (method.ReturnType == typeof(void))
                        {
                            return Behavior.None();
                        }

                        Tuple<MethodInfo, CallData> key = Tuple.Create(method, callData);
                        lock (resultLock)
                        {
                            if (!resultCache.TryGetValue(key, out Behavior? behavior))
                            {
                                if (method.ReturnType.IsInheritedBy<string>())
                                {
                                    behavior = Behavior.Returns(
                                        smartData.Find(method.Name) ?? randomizer.Create<string>()
                                    );
                                }
                                else
                                {
                                    behavior = Behavior.Returns(
                                        randomizer.Create(method.ReturnType)
                                    );
                                }
                                resultCache.Add(key, behavior);
                            }
                            return behavior;
                        }
                    },
                }
        );

        return (Fake)type.GetConstructor([typeof(Fake)])!.Invoke([mock]);
    }
}
