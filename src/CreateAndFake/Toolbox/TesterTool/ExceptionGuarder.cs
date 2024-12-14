using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Automates basic layer passthrough checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class ExceptionGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <inheritdoc cref="BaseGuarder.CallAllMethods(MethodBase,ParameterInfo,object,ICollection{object})"/>
    internal void CallAllMethods(object instance, ICollection<object?>? injectionValues)
    {
        CallAllMethods(null, null, instance, injectionValues);
    }

    /// <inheritdoc/>
    protected override void HandleCheckException(MethodBase testOrigin, ParameterInfo? testParam, Exception taskException)
    {
        ArgumentGuard.ThrowIfNull(testOrigin, nameof(testOrigin));
        ArgumentGuard.ThrowIfNull(taskException, nameof(taskException));

        Options.Asserter.Fail(taskException, $"Exception encountered on method '{testOrigin.Name}'.");
    }
}
