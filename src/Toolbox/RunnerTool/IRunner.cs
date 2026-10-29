global using RunnerMod = System.Func<
    Werecodent.CreateAndFake.RunnerTool.RunnerOptions,
    Werecodent.CreateAndFake.RunnerTool.RunnerOptions
>;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.RunnerTool;

/// <summary>Tool for generating method data and running them.</summary>
/// <remarks>Useful for generating tests and invoking methods at runtime.</remarks>
public interface IRunner : ITool<RunnerOptions>
{
    /// <summary>Creates a new tool with the <paramref name="optionConfiguration"/> changes applied.</summary>
    /// <param name="optionConfiguration">Modifications of this tool's options for the new tool.</param>
    /// <returns>The created tool.</returns>
    IRunner WithOptions(RunnerMod optionConfiguration);

    /// <summary>Calls all methods on the <paramref name="instance"/>.</summary>
    /// <param name="instance">Instance whose methods to call.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>Results of the method calls.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="RunnerOptions.RunTimeout"/></item>
    ///         <item><see cref="RunnerOptions.MethodsToIgnore"/></item>
    ///         <item><see cref="RunnerOptions.IncludeBaseObjectMethods"/></item>
    ///     </list>
    /// </remarks>
    Task<RunResults> CallMethodsOnAsync(
        object instance,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    );

    /// <inheritdoc cref="CreateForAsync"/>
    MethodCallWrapper CreateFor(
        MethodBase method,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    );

    /// <summary>
    ///     Constructs parameter data for the <paramref name="method"/> utilizing
    ///     attached <see cref="ParameterHintAttribute"/>s as instructions.
    /// </summary>
    /// <param name="method">Method to create parameters for.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>Parameter arguments for <paramref name="method"/> in order.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="RunnerOptions.InjectionValues"/></item>
    ///     </list>
    /// </remarks>
    Task<MethodCallWrapper> CreateForAsync(
        MethodBase method,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    );

    /// <summary>
    ///     Constructs parameter data for the <paramref name="method"/> and runs it on the <paramref name="instance"/>.
    /// </summary>
    /// <param name="method">Method to create parameter data for and run.</param>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="RunnerOptions.RunTimeout"/></item>
    ///         <item><see cref="RunnerOptions.InjectionValues"/></item>
    ///     </list>
    /// </remarks>
    /// <inheritdoc cref="RunAsync(object,MethodCallWrapper,CancellationToken,RunnerMod)"/>
    Task<RunResult> RunAsync(
        object? instance,
        MethodInfo method,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    );

    /// <summary>Runs the method with constructed parameter data on the instance.</summary>
    /// <param name="instance">Instance to run on.</param>
    /// <param name="data">Method with parameter data to run.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <param name="optionConfiguration">Modifications of this tool's options to apply for this call.</param>
    /// <returns>Results of the run.</returns>
    /// <remarks>
    ///     Important relevant options: <list type="bullet">
    ///         <item><see cref="RunnerOptions.RunTimeout"/></item>
    ///     </list>
    /// </remarks>
    Task<RunResult> RunAsync(
        object? instance,
        MethodCallWrapper data,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    );
}
