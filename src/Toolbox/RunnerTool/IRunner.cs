global using RunnerMod = System.Func<
    CreateAndFake.RunnerTool.RunnerOptions,
    CreateAndFake.RunnerTool.RunnerOptions
>;
using System.Reflection;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.RunnerTool;

/// <summary>Creates objects and populates them with random values.</summary>
public interface IRunner : ITool<RunnerOptions>
{
    /// <summary>Calls all methods of <paramref name="instance"/>.</summary>
    /// <param name="instance">Instance whose methods to call.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <returns>Results of the method calls.</returns>
    Task<RunResults> CallMethodsOn(object instance, RunnerMod? optionConfiguration = null);

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
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <param name="values">Starting values to inject into instances.</param>
    /// <returns>Parameter arguments for <paramref name="method"/> in order.</returns>
    MethodCallWrapper CreateFor(
        MethodBase method,
        RunnerMod optionConfiguration,
        params IEnumerable<object?>? values
    );

    /// <summary>Runs the given method on the instance.</summary>
    /// <param name="instance">Instance to run on.</param>
    /// <param name="method">Method to run.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <returns>Results of the run.</returns>
    Task<RunResult> Run(object? instance, MethodInfo method, RunnerMod? optionConfiguration = null);

    // <summary>Runs the given method on the instance.</summary>
    /// <param name="instance">Instance to run on.</param>
    /// <param name="data">Method to run.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="ITool{T}.Options"/> to apply for this call.</param>
    /// <returns>Results of the run.</returns>
    Task<RunResult> Run(
        object? instance,
        MethodCallWrapper data,
        RunnerMod? optionConfiguration = null
    );
}
