using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.TesterTool;

/// <summary>Automates null reference guard checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class NullGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <summary>
    ///     Verifies nulls are guarded on constructors.
    ///     Tests each nullable parameter possible with null.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate constructor parameters.</param>
    internal async Task PreventsNullRefExceptionOnConstructors(Type type, bool callAllMethods)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (ConstructorInfo constructor in FindAllConstructors(type))
        {
            await PreventsNullRefException(null, constructor, callAllMethods).ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Verifies nulls are guarded on methods.
    ///     Tests each nullable parameter possible with null.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="instance">Instance to test the methods on.</param>
    internal async Task PreventsNullRefExceptionOnMethods(object instance)
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        foreach (MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance))
        {
            await PreventsNullRefException(instance, GenericFixer.FixMethod(method, Options), false)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Verifies nulls are guarded on methods.
    ///     Tests each nullable parameter possible with null.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate factory parameters.</param>
    internal async Task PreventsNullRefExceptionOnStatics(Type type, bool callAllMethods)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (MethodInfo method in FindAllMethods(type, BindingFlags.Static))
        {
            await PreventsNullRefException(
                    null,
                    GenericFixer.FixMethod(method, Options),
                    callAllMethods && method.ReturnType.Inherits(type)
                )
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies nulls are guarded on a method.</summary>
    /// <param name="instance">Instance with the method under test.</param>
    /// <param name="method">Method under test.</param>
    /// <param name="callAllMethods">If all instance methods should be called after the method.</param>
    private async Task PreventsNullRefException(
        object? instance,
        MethodBase method,
        bool callAllMethods
    )
    {
        MethodCallWrapper? data = null;
        try
        {
            data = Options.Runner.CreateFor(method, Options.InjectionValues);

            for (int i = 0; i < data.ArgCount; i++)
            {
                ParameterInfo param = method.GetParameters()[i];
                if (
                    param.ParameterType.IsValueType
                    && Nullable.GetUnderlyingType(param.ParameterType) == null
                )
                {
                    continue;
                }

                object? original = data.ModifyArg(i, null);
                object? result = null;
                try
                {
                    result = await RunCheck(method, param, instance, data).ConfigureAwait(false);

                    if (result != null && callAllMethods)
                    {
                        await CallAllMethods(method, param, result).ConfigureAwait(false);
                    }
                }
                finally
                {
                    _ = data.ModifyArg(i, original);
                    await DisposeAllButInjected(result).ConfigureAwait(false);
                }
            }
        }
        finally
        {
            await DisposeAllButInjected(data).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    protected override bool HandleCheckException(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        Exception taskException
    )
    {
        ArgumentGuard.ThrowIfNull(testOrigin, nameof(testOrigin));
        ArgumentGuard.ThrowIfNull(testParam, nameof(testParam));
        ArgumentGuard.ThrowIfNull(taskException, nameof(taskException));

        string details = $"on method '{testOrigin.Name}' with parameter '{testParam.Name}'";

        if (taskException is NullReferenceException)
        {
            Options.Asserter.Fail(
                taskException,
                $"Null reference exception encountered {details}."
            );
        }

        return Options.IgnoreAllExceptions
            || Options.IgnorableExceptions.Contains(taskException.GetType())
            || taskException.GetType() == typeof(ArgumentNullException);
    }
}
