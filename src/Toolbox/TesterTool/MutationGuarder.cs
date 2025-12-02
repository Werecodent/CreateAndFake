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
    internal async Task PreventsMutationOnConstructors(Type type, bool callAllMethods)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (ConstructorInfo constructor in FindAllConstructors(type))
        {
            await PreventsMutation(null, constructor, callAllMethods).ConfigureAwait(false);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    internal async Task PreventsMutationOnMethods(object instance)
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        foreach (MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance))
        {
            await PreventsMutation(instance, GenericFixer.FixMethod(method, Options), false)
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate factory parameters.</param>
    internal async Task PreventsMutationOnStatics(Type type, bool callAllMethods)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (MethodInfo method in FindAllMethods(type, BindingFlags.Static))
        {
            await PreventsMutation(
                    null,
                    GenericFixer.FixMethod(method, Options),
                    callAllMethods && method.ReturnType.Inherits(type)
                )
                .ConfigureAwait(false);
        }
    }

    /// <summary>Verifies mutations are prevented on a method.</summary>
    /// <param name="instance">Instance with the method under test.</param>
    /// <param name="method">Method under test.</param>
    /// <param name="callAllMethods">If all instance methods should be called after the method.</param>
    private async Task PreventsMutation(object? instance, MethodBase method, bool callAllMethods)
    {
        MethodCallWrapper? data = null;
        MethodCallWrapper? copy = null;
        object? result = null;
        try
        {
            data = Options.Runner.CreateFor(method, Options.InjectionValues);
            copy = Options.Duplicator.Copy(data);

            result = await RunCheck(method, null, instance, data).ConfigureAwait(false);

            if (result != null && callAllMethods)
            {
                await CallAllMethods(method, null, result).ConfigureAwait(false);
            }

            await Options
                .AsyncAsserter.ValuesEqual(
                    copy,
                    data,
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
        ArgumentGuard.ThrowIfNull(taskException, nameof(taskException));

        return Options.IgnoreAllExceptions
            || Options.IgnorableExceptions.Contains(taskException.GetType());
    }
}
