using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.TesterTool;

/// <summary>Automates parameter mutation checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class MutationGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <summary>Verifies mutations are prevented on constructors.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate constructor parameters.</param>
    internal void PreventsMutationOnConstructors(Type type, bool callAllMethods)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (ConstructorInfo constructor in FindAllConstructors(type))
        {
            PreventsMutation(null, constructor, callAllMethods);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    internal void PreventsMutationOnMethods(object instance)
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        foreach (MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance))
        {
            PreventsMutation(instance, GenericFixer.FixMethod(method, Options), false);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate factory parameters.</param>
    internal void PreventsMutationOnStatics(Type type, bool callAllMethods)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (MethodInfo method in FindAllMethods(type, BindingFlags.Static))
        {
            PreventsMutation(null, GenericFixer.FixMethod(method, Options),
                callAllMethods && method.ReturnType.Inherits(type));
        }
    }

    /// <summary>Verifies mutations are prevented on a method.</summary>
    /// <param name="instance">Instance with the method under test.</param>
    /// <param name="method">Method under test.</param>
    /// <param name="callAllMethods">If all instance methods should be called after the method.</param>
    private void PreventsMutation(object? instance, MethodBase method, bool callAllMethods)
    {
        object?[]? data = null;
        object?[]? copy = null;
        object? result = null;
        try
        {
            data = [.. Options.Randomizer.CreateFor(method, Options.InjectionValues).Args];
            copy = Options.Duplicator.Copy(data);

            result = (instance == null && method is ConstructorInfo builder)
                ? RunCheck(method, null, () => builder.Invoke(data))
                : RunCheck(method, null, () => method.Invoke(instance, data!)!);

            if (result != null && callAllMethods)
            {
                CallAllMethods(method, null, result);
            }

            Options.Asserter.ValuesEqual(copy, data, $"Parameter data was mutated when testing '{method.Name}'.");
        }
        finally
        {
            DisposeAllButInjected(data);
            DisposeAllButInjected(copy);
            DisposeAllButInjected(result);
        }
    }

    /// <inheritdoc/>
    protected override bool HandleCheckException(
        MethodBase testOrigin, ParameterInfo? testParam, Exception taskException)
    {
        ArgumentGuard.ThrowIfNull(taskException, nameof(taskException));

        return Options.IgnorableExceptions.Contains(taskException.GetType());
    }
}
