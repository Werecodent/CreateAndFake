using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Automates parameter mutation checks.</summary>
/// <param name="options"><inheritdoc cref="BaseGuarder.Options" path="/summary"/></param>
internal sealed class MutationGuarder(TesterOptions options) : BaseGuarder(options)
{
    /// <summary>Verifies mutations are prevented on constructors.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate constructor parameters.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    internal void PreventsMutationOnConstructors(Type type, bool callAllMethods, ICollection<object?>? injectionValues)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (ConstructorInfo constructor in FindAllConstructors(type))
        {
            PreventsMutation(null, constructor, callAllMethods, injectionValues);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    internal void PreventsMutationOnMethods(object instance, ICollection<object?>? injectionValues)
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        foreach (MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance)
            .Where(m => m.Name is not "Finalize" and not "Dispose"))
        {
            PreventsMutation(instance, GenericFixer.FixMethod(method, Options), false, injectionValues);
        }
    }

    /// <summary>Verifies mutations are prevented on methods.</summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate factory parameters.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    internal void PreventsMutationOnStatics(Type type, bool callAllMethods, ICollection<object?>? injectionValues)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (MethodInfo method in FindAllMethods(type, BindingFlags.Static))
        {
            PreventsMutation(null, GenericFixer.FixMethod(method, Options),
                callAllMethods && method.ReturnType.Inherits(type), injectionValues);
        }
    }

    /// <summary>Verifies mutations are prevented on a method.</summary>
    /// <param name="instance">Instance with the method under test.</param>
    /// <param name="method">Method under test.</param>
    /// <param name="callAllMethods">If all instance methods should be called after the method.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    private void PreventsMutation(object? instance,
        MethodBase method, bool callAllMethods, ICollection<object?>? injectionValues)
    {
        object?[]? data = null;
        object?[]? copy = null;
        object? result = null;
        try
        {
            data = Options.Randomizer.CreateFor(method, injectionValues).Args.ToArray();
            copy = Options.Duplicator.Copy(data);

            result = (instance == null && method is ConstructorInfo builder)
                ? RunCheck(method, null, () => builder.Invoke(data))
                : RunCheck(method, null, () => method.Invoke(instance, data!)!);

            if (result != null && callAllMethods)
            {
                CallAllMethods(method, null, result, injectionValues);
            }

            Options.Asserter.ValuesEqual(copy, data, $"Parameter data was mutated when testing '{method.Name}'.");
        }
        finally
        {
            DisposeAllButInjected(data, injectionValues);
            DisposeAllButInjected(copy, injectionValues);
            DisposeAllButInjected(result, injectionValues);
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
