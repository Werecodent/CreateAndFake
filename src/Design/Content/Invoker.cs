using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Provides common patterns around executing code.</summary>
public static class Invoker
{
    /// <summary>Ensures the result is completed.</summary>
    /// <param name="task">Potentially wrapped data.</param>
    /// <returns>The unwrapped result.</returns>
    public static async Task<object?> AwaitAsync(Task task)
    {
        ArgumentGuard.ThrowIfNull(task);

        await task.ConfigureAwait(false);

        Type resultType = task.GetType();
        PropertyInfo? resultProp = TypeDescriber
            .For(resultType)
            .Properties.OnlyPublic.FirstOrDefault(p => p.Name == "Result");

        if (
            !GenericConverter
                .ExpandName(resultType)
                .Contains("VoidTaskResult", StringComparison.Ordinal)
            && resultProp != null
        )
        {
            // await ((dynamic)result) crashes legacy .NET.
            return resultProp.GetValue(task);
        }
        else
        {
            return VoidType.Instance;
        }
    }
}
