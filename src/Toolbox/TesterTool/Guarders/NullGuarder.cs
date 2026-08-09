using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.TesterTool.Guarders;

/// <summary>Automates null reference guard checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class NullGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    public async Task PreventsNullRefExceptionAsync(Type type, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(type);

        if (Options.DisableNullRefExceptionTests)
        {
            return;
        }

        if (Options.IncludeConstructors)
        {
            await PreventsNullRefExceptionOnConstructorsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }

        await CreateInstanceAndTestMethodsAsync(
                type,
                PreventsNullRefExceptionOnMethodsAsync,
                canceler
            )
            .ConfigureAwait(false);

        if (Options.IncludeStaticMethods)
        {
            await PreventsNullRefExceptionOnStaticsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc cref="ITester.PreventsNullRefExceptionAsync"/>
    public async Task PreventsNullRefExceptionAsync<T>(T instance, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(instance);

        if (Options.DisableNullRefExceptionTests)
        {
            return;
        }

        if (Options.IncludeConstructors)
        {
            await PreventsNullRefExceptionOnConstructorsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
        if (Options.IncludeInstanceMethods)
        {
            await PreventsNullRefExceptionOnMethodsAsync(instance, canceler).ConfigureAwait(false);
        }
        if (Options.IncludeStaticMethods)
        {
            await PreventsNullRefExceptionOnStaticsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Verifies nulls are guarded on constructors.
    ///     Tests each nullable parameter possible with null.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate constructor parameters.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    internal async Task PreventsNullRefExceptionOnConstructorsAsync(
        Type type,
        bool callAllMethods,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(type);

        foreach (ConstructorInfo constructor in FindAllConstructors(type))
        {
            await PreventsNullRefExceptionAsync(null, constructor, callAllMethods, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Verifies nulls are guarded on methods.
    ///     Tests each nullable parameter possible with null.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    internal async Task PreventsNullRefExceptionOnMethodsAsync(
        object instance,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(instance);

        foreach (MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance))
        {
            await PreventsNullRefExceptionAsync(
                    instance,
                    GenericFixer.FixMethod(method, Options),
                    false,
                    canceler
                )
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
    /// <param name="canceler">Aborts execution if triggered.</param>
    internal async Task PreventsNullRefExceptionOnStaticsAsync(
        Type type,
        bool callAllMethods,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(type);

        foreach (MethodInfo method in FindAllMethods(type, BindingFlags.Static))
        {
            await PreventsNullRefExceptionAsync(
                    null,
                    GenericFixer.FixMethod(method, Options),
                    callAllMethods && method.ReturnType.Inherits(type),
                    canceler
                )
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies nulls are guarded on a method.</summary>
    /// <param name="instance">Instance with the method under test.</param>
    /// <param name="method">Method under test.</param>
    /// <param name="callAllMethods">If all instance methods should be called after the method.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    private async Task PreventsNullRefExceptionAsync(
        object? instance,
        MethodBase method,
        bool callAllMethods,
        CancellationToken canceler
    )
    {
        using CancellationTokenSource cleanupCanceler =
            CancellationTokenSource.CreateLinkedTokenSource(canceler);

        MethodCallWrapper? data = null;
        try
        {
            data = Options.Runner.CreateFor(method, cleanupCanceler.Token, Options.InjectionValues);

            bool called = false;
            for (int i = 0; i < data.ArgCount; i++)
            {
                ParameterInfo param = method.GetParameters()[i];
                if (
                    param.IsOut
                    || (
                        param.ParameterType.IsValueType
                        && Nullable.GetUnderlyingType(param.ParameterType) == null
                    )
                )
                {
                    continue;
                }

                object? original = data.ModifyArg(i, null);
                object? result = null;
                try
                {
                    result = await RunCheckAsync(
                            method,
                            param,
                            instance,
                            data,
                            cleanupCanceler.Token
                        )
                        .ConfigureAwait(false);

                    called = true;

                    if (result != null && callAllMethods)
                    {
                        await CallAllMethodsAsync(method, param, result, cleanupCanceler.Token)
                            .ConfigureAwait(false);
                    }
                }
                finally
                {
                    _ = data.ModifyArg(i, original);
                    await DisposeAllButInjectedAsync(result).ConfigureAwait(false);
                }
            }
            if (!called)
            {
                await DisposeAllButInjectedAsync(
                        await RunCheckAsync(method, null, instance, data, cleanupCanceler.Token)
                            .ConfigureAwait(false)
                    )
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            await AsyncSeriesHelper.TriggerCancellationAsync(cleanupCanceler).ConfigureAwait(false);
            await DisposeAllButInjectedAsync(data?.Args).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    protected override bool HandleCheckException(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        Exception taskException
    )
    {
        ArgumentGuard.ThrowIfNull(testOrigin, taskException);

        string details = $"on method '{testOrigin.Name}' with parameter '{testParam?.Name}'";

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
