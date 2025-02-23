global using RunnerMod = System.Func<
    CreateAndFake.RunnerTool.RunnerOptions,
    CreateAndFake.RunnerTool.RunnerOptions>;
using System.Reflection;

namespace CreateAndFake.RunnerTool;

/// <summary>Creates objects and populates them with random values.</summary>
public interface IRunner
{
    /// <summary>Configured options for <c>this</c>.</summary>
    RunnerOptions Options { get; }

    /// <summary>
    ///     Constructs the parameters for <paramref name="method"/>.
    ///     Randomizes types by default.
    ///     Earlier types will be used to construct later types if possible.
    /// </summary>
    /// <param name="method">Method to create parameters for.</param>
    /// <param name="values">Starting values to inject into instances.</param>
    /// <returns>Parameter arguments for <paramref name="method"/> in order.</returns>
    MethodCallWrapper CreateFor(MethodBase method, params IEnumerable<object?>? values);

    /// <summary>
    ///     Constructs the parameters for <paramref name="method"/>.
    ///     Randomizes types by default.
    ///     Earlier types will be used to construct later types if possible.
    /// </summary>
    /// <param name="method">Method to create parameters for.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> to apply for this call.</param>
    /// <param name="values">Starting values to inject into instances.</param>
    /// <returns>Parameter arguments for <paramref name="method"/> in order.</returns>
    MethodCallWrapper CreateFor(MethodBase method,
        RunnerMod optionConfiguration, params IEnumerable<object?>? values);
}
