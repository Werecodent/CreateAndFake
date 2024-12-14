using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.TesterTool;

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
    /// <param name="injectionValues">Values to inject into the method.</param>
    internal void PreventsNullRefExceptionOnConstructors(
        Type type, bool callAllMethods, ICollection<object?>? injectionValues)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (ConstructorInfo constructor in FindAllConstructors(type))
        {
            PreventsNullRefException(null, constructor, callAllMethods, injectionValues);
        }
    }

    /// <summary>
    ///     Verifies nulls are guarded on methods.
    ///     Tests each nullable parameter possible with null.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    internal void PreventsNullRefExceptionOnMethods(object instance, ICollection<object?>? injectionValues)
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        foreach (MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance)
            .Where(m => m.Name is not "Finalize" and not "Dispose"))
        {
            PreventsNullRefException(instance, GenericFixer.FixMethod(method, Options), false, injectionValues);
        }
    }

    /// <summary>
    ///     Verifies nulls are guarded on methods.
    ///     Tests each nullable parameter possible with null.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="callAllMethods">Run instance methods to validate factory parameters.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    internal void PreventsNullRefExceptionOnStatics(
        Type type, bool callAllMethods, ICollection<object?>? injectionValues)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        foreach (MethodInfo method in FindAllMethods(type, BindingFlags.Static))
        {
            PreventsNullRefException(null, GenericFixer.FixMethod(method, Options),
                callAllMethods && method.ReturnType.Inherits(type), injectionValues);
        }
    }

    /// <summary>Verifies nulls are guarded on a method.</summary>
    /// <param name="instance">Instance with the method under test.</param>
    /// <param name="method">Method under test.</param>
    /// <param name="callAllMethods">If all instance methods should be called after the method.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    private void PreventsNullRefException(object? instance,
        MethodBase method, bool callAllMethods, ICollection<object?>? injectionValues)
    {
        object?[]? data = null;
        object? result = null;
        try
        {
            data = Options.Randomizer.CreateFor(method, injectionValues).Args.ToArray();

            for (int i = 0; i < data.Length; i++)
            {
                ParameterInfo param = method.GetParameters()[i];
                if (param.ParameterType.IsValueType
                    && Nullable.GetUnderlyingType(param.ParameterType) == null)
                {
                    continue;
                }

                object? original = data[i];
                data[i] = null;
                try
                {
                    result = (instance == null && method is ConstructorInfo builder)
                        ? RunCheck(method, param, () => builder.Invoke(data))
                        : RunCheck(method, param, () => method.Invoke(instance, data)!);

                    if (result != null && callAllMethods)
                    {
                        CallAllMethods(method, param, result, injectionValues);
                    }
                }
                finally
                {
                    data[i] = original;
                }
            }
        }
        finally
        {
            DisposeAllButInjected(data, injectionValues);
            DisposeAllButInjected(result, injectionValues);
        }
    }

    /// <inheritdoc/>
    protected override void HandleCheckException(MethodBase testOrigin,
        ParameterInfo? testParam, Exception taskException)
    {
        ArgumentGuard.ThrowIfNull(testOrigin, nameof(testOrigin));
        ArgumentGuard.ThrowIfNull(testParam, nameof(testParam));
        ArgumentGuard.ThrowIfNull(taskException, nameof(taskException));

        string details = $"on method '{testOrigin.Name}' with parameter '{testParam.Name}'";

        Options.Asserter.Is(false, taskException is NullReferenceException,
            $"Null reference exception encountered {details}.");
    }
}
