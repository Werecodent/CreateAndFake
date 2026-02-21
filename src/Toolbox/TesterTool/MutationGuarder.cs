using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.TesterTool;

/// <summary>Automates parameter mutation checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class MutationGuarder(TesterOptions options) : BaseGuarder(options)
{
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
        MethodCallWrapper? data = null;
        MethodCallWrapper? copy = null;
        object? result = null;
        try
        {
            data = Options.Runner.CreateFor(method, Options.InjectionValues);
            copy = Options.Duplicator.Copy(data);

            result = await RunCheckAsync(method, null, instance, data, canceler)
                .ConfigureAwait(false);

            if (result != null && callAllMethods)
            {
                await CallAllMethodsAsync(method, null, result, canceler).ConfigureAwait(false);
            }

            await Options
                .Asserter.ValuesEqualAsync(
                    copy,
                    data,
                    canceler,
                    $"Parameter data was mutated when testing '{method.Name}'."
                )
                .ConfigureAwait(false);
        }
        finally
        {
            await DisposeAllButInjected(data?.Args).ConfigureAwait(false);
            await DisposeAllButInjected(copy?.Args).ConfigureAwait(false);
            await DisposeAllButInjected(result).ConfigureAwait(false);
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
