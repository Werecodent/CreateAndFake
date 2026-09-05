using System.Collections.Specialized;
using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached <see langword="object"/> as a deep clone of a previous parameter.</summary>.
/// <seealso cref="IRunner.CreateFor(MethodBase,CancellationToken,RunnerMod)"/>
/// <seealso cref="DuplicatorTool.IDuplicator.Copy"/>
public abstract class BaseCopyAttribute : ParameterHintAttribute
{
    /// <inheritdoc/>
    protected internal override object? CreateParameterValue(
        ParameterInfo param,
        MethodBase method,
        OrderedDictionary args,
        RunnerOptions localOptions
    )
    {
        ArgumentGuard.ThrowIfNull(param, method, args, localOptions);

        ParameterInfo[] methodParams = method.GetParameters();

        int current = Array.FindIndex(methodParams, p => p.Name == param.Name);
        if (current < 0)
        {
            throw new ToolException($"Parameter '{param}' was not found on '{method}'.");
        }

        for (int i = current - 1; i >= 0; i--)
        {
            if (param.ParameterType == methodParams[i].ParameterType)
            {
                return localOptions.Duplicator.Copy(args[i])!;
            }
        }

        throw new ToolException($"No value to copy for '{param}' on '{method}'.");
    }
}
