using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ValueCreateHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICreateHandler> Handlers =>
        ValueRandom.SupportedTypes.SelectMany<Type, ICreateHandler>(t =>
            [
                new FactoryCreateHandler(t, rand => rand.Options.Gen.Next(t)),
                new FactoryCreateHandler(typeof(Nullable<>).MakeGenericType(t), rand =>
                    rand.Options.Gen.Next(t)
                ),
            ]
        );
}
