using System.Collections.Specialized;
using System.Reflection;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <summary>Flag to customize behavior for the attached parameter value during random data generation.</summary>
/// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public abstract class ParameterHintAttribute : Attribute
{
    /// <summary>Manages creating the value for the attached parameter.</summary>
    /// <param name="param">Info for the attached parameter.</param>
    /// <param name="method">Originating method for the parameter.</param>
    /// <param name="args">Data already generated for the previous parameters of the method.</param>
    /// <param name="localOptions">Potentially modified configuration to use.</param>
    /// <returns>The created parameter value.</returns>
    protected internal abstract object? CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    );

    /// <inheritdoc cref="CreateParameterValue"/>
    protected internal virtual Task<object?> CreateParameterValueAsync(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        return Task.FromResult(CreateParameterValue(param, method, args, localOptions));
    }
}

#pragma warning restore
