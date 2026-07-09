using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool;

namespace CreateAndFake.TesterTool.Guarders;

/// <summary>Automates basic layer passthrough checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class ExceptionGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <inheritdoc cref="ITester.PassthroughWithNoExceptionsAsync"/>
    public Task PassthroughWithNoExceptionsAsync<T>(CancellationToken canceler)
    {
        if (Options.DisablePassthroughTests)
        {
            return Task.CompletedTask;
        }

        object instance = Options.Randomizer.Create<Injected<T>>()!.Dummy!;
        return new ExceptionGuarder(Options).CallAllMethodsAsync(instance, canceler);
    }

    /// <inheritdoc cref="ITester.PassthroughWithNoExceptionsAsync"/>
    public Task PassthroughWithNoExceptionsAsync(object instance, CancellationToken canceler)
    {
        if (Options.DisablePassthroughTests)
        {
            return Task.CompletedTask;
        }

        return CallAllMethodsAsync(instance, canceler);
    }

    /// <inheritdoc cref="BaseGuarder.CallAllMethodsAsync(MethodBase,ParameterInfo,object,CancellationToken)"/>
    internal async Task CallAllMethodsAsync(object instance, CancellationToken canceler)
    {
        using CancellationTokenSource cleanupCanceler =
            CancellationTokenSource.CreateLinkedTokenSource(canceler);

        try
        {
            await CallAllMethodsAsync(null, null, instance, canceler).ConfigureAwait(false);
        }
        finally
        {
            await AsyncSeriesHelper.TriggerCancellationAsync(cleanupCanceler).ConfigureAwait(false);
        }
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

        if (
            !Options.IgnorableExceptions.Contains(taskException.GetType())
            && !Options.IgnoreAllExceptions
        )
        {
            Options.Asserter.Fail(
                taskException,
                $"Exception encountered on method '{testOrigin.Name}'."
            );
        }
        return true;
    }
}
