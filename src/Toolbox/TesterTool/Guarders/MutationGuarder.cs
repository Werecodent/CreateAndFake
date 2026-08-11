using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.TesterTool.Guarders;

/// <summary>Automates parameter mutation checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class MutationGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <inheritdoc cref="ITester.PreventsParameterMutationAsync"/>
    public async Task PreventsParameterMutationAsync(Type type, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(type);

        if (Options.DisableParameterMutationTests)
        {
            return;
        }

        MutationGuarder checker = new(Options);

        if (Options.IncludeConstructors)
        {
            await checker
                .PreventsMutationOnConstructorsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }

        await CreateInstanceAndTestMethodsAsync(
                type,
                checker.PreventsMutationOnMethodsAsync,
                canceler
            )
            .ConfigureAwait(false);

        if (Options.IncludeStaticMethods)
        {
            await checker
                .PreventsMutationOnStaticsAsync(type, true, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutationAsync"/>
    public async Task PreventsParameterMutationAsync<T>(T instance, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(instance);

        if (Options.DisableParameterMutationTests)
        {
            return;
        }

        MutationGuarder checker = new(Options);

        if (Options.IncludeConstructors)
        {
            await checker
                .PreventsMutationOnConstructorsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
        if (Options.IncludeInstanceMethods)
        {
            await checker.PreventsMutationOnMethodsAsync(instance, canceler).ConfigureAwait(false);
        }
        if (Options.IncludeStaticMethods)
        {
            await checker
                .PreventsMutationOnStaticsAsync(typeof(T), false, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies mutations are prevented on constructors.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate constructor parameters.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    internal async Task PreventsMutationOnConstructorsAsync(
        Type type,
        bool callAllMethods,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(type);

        foreach (
            ConstructorInfo constructor in FindAllConstructors(GenericFixer.FixType(type, Options))
                .Where(c => c.GetParameters().Length > 0)
        )
        {
            await PreventsMutationAsync(null, constructor, callAllMethods, canceler)
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    internal async Task PreventsMutationOnMethodsAsync(object instance, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(instance);

        foreach (
            MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance)
                .Where(c => c.GetParameters().Length > 0)
        )
        {
            await PreventsMutationAsync(
                    instance,
                    GenericFixer.FixMethod(method, Options),
                    false,
                    canceler
                )
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate factory parameters.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    internal async Task PreventsMutationOnStaticsAsync(
        Type type,
        bool callAllMethods,
        CancellationToken canceler
    )
    {
        Type specificType = GenericFixer.FixType(type, Options);

        foreach (
            MethodInfo method in FindAllMethods(specificType, BindingFlags.Static)
                .Where(c => c.GetParameters().Length > 0)
        )
        {
            await PreventsMutationAsync(
                    null,
                    GenericFixer.FixMethod(method, Options),
                    callAllMethods && method.ReturnType.Inherits(specificType),
                    canceler
                )
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies mutations are prevented on a method.</summary>
    /// <param name="instance">Instance with the method under test.</param>
    /// <param name="method">Method under test.</param>
    /// <param name="callAllMethods">If all instance methods should be called after the method.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    private async Task PreventsMutationAsync(
        object? instance,
        MethodBase method,
        bool callAllMethods,
        CancellationToken canceler
    )
    {
        using CancellationTokenSource cleanupCanceler =
            CancellationTokenSource.CreateLinkedTokenSource(canceler);

        MethodCallWrapper? data = null;
        MethodCallWrapper? copy = null;
        object? result = null;
        try
        {
            data = Options.Runner.CreateFor(
                method,
                opt => opt with { InjectionValues = Options.InjectionValues },
                cleanupCanceler.Token
            );
            copy = Options.Duplicator.Copy(data);

            result = await RunCheckAsync(method, null, instance, data, cleanupCanceler.Token)
                .ConfigureAwait(false);

            if (result != null && callAllMethods)
            {
                await CallAllMethodsAsync(method, null, result, cleanupCanceler.Token)
                    .ConfigureAwait(false);
            }

            await Options
                .Asserter.ValuesEqualAsync(
                    copy,
                    data,
                    cleanupCanceler.Token,
                    $"Parameter data was mutated when testing '{GenericConverter.BuildTestName(method)}'."
                )
                .ConfigureAwait(false);
        }
        finally
        {
            await AsyncSeriesHelper.TriggerCancellationAsync(cleanupCanceler).ConfigureAwait(false);
            await DisposeAllButInjectedAsync(data?.Args).ConfigureAwait(false);
            await DisposeAllButInjectedAsync(copy?.Args).ConfigureAwait(false);
            await DisposeAllButInjectedAsync(result).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    protected override bool HandleCheckException(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        Exception taskException
    )
    {
        ArgumentGuard.ThrowIfNull(taskException);

        return Options.IgnoreAllExceptions
            || Options.IgnorableExceptions.Contains(taskException.GetType());
    }
}
