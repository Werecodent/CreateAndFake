using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.TesterTool;

/// <summary>Handles generic resolution.</summary>
internal static class GenericFixer
{
    /// <summary>Defines any generics in a type.</summary>
    /// <param name="type">Type to fix.</param>
    /// <param name="options"></param>
    /// <returns>Type with all generics defined.</returns>
    internal static Type FixType(Type type, TesterOptions options)
    {
        ArgumentGuard.ThrowIfNull(options);

        return GenericResolver.OfConcrete(
            type,
            new RandomizerChainer(options.Randomizer.Options, new RandomizerEngine())
        );
    }

    /// <summary>Defines any generics in a method.</summary>
    /// <param name="method">Method to fix.</param>
    /// <param name="options"></param>
    /// <returns>Method with all generics defined.</returns>
    internal static MethodInfo FixMethod(MethodInfo method, TesterOptions options)
    {
        ArgumentGuard.ThrowIfNull(options);

        return GenericResolver.OfConcrete(method, options.Randomizer);
    }
}
