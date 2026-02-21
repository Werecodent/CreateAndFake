using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.TesterTool;

/// <summary>Automates basic layer passthrough checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class ExceptionGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <inheritdoc cref="BaseGuarder.CallAllMethodsAsync(MethodBase,ParameterInfo,object,CancellationToken)"/>
    internal Task CallAllMethodsAsync(object instance, CancellationToken canceler)
    {
        return CallAllMethodsAsync(null, null, instance, canceler);
    }

    /// <inheritdoc/>
    protected override bool HandleCheckException(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        Exception taskException
    )
    {
        ArgumentGuard.ThrowIfNull(testOrigin);
        ArgumentGuard.ThrowIfNull(taskException);

        Options.Asserter.Fail(
            taskException,
            $"Exception encountered on method '{testOrigin.Name}'."
        );
        return true;
    }
}
