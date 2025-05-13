using System.Collections;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RunnerTool;

#pragma warning disable CA1822 // Member does not access instance data and can be marked static

namespace CreateAndFake.TesterTool;

/// <summary>Automates checks.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
internal abstract class BaseGuarder(TesterOptions options)
{
    /// <summary>Configured options for testing.</summary>
    protected TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>Gets all testable constructors on a type.</summary>
    /// <param name="type">Type with the constructors to test.</param>
    /// <returns>Found constructors.</returns>
    protected IEnumerable<ConstructorInfo> FindAllConstructors(Type type)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        BindingFlags scope = Options.IncludeInternals
            ? BindingFlags.Public | BindingFlags.NonPublic
            : BindingFlags.Public;
        return type.GetConstructors(BindingFlags.Instance | scope)
            .Where(c => c.IsPublic || c.IsAssembly || c.IsFamilyOrAssembly)
            .Where(c => !c.IsPrivate);
    }

    /// <summary>Gets all testable methods on a type.</summary>
    /// <param name="type">Type with the methods to test.</param>
    /// <param name="kind">Instance, static, or both.</param>
    /// <returns>Found methods.</returns>
    protected IEnumerable<MethodInfo> FindAllMethods(Type type, BindingFlags kind)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        BindingFlags scope = Options.IncludeInternals
            ? BindingFlags.Public | BindingFlags.NonPublic
            : BindingFlags.Public;
        return type.GetMethods(kind | scope)
            .Where(m => m.IsPublic || m.IsAssembly || m.IsFamily || m.IsFamilyOrAssembly)
            .Where(m => m.DeclaringType == type || m.DeclaringType!.IsAbstract)
            .Where(m => !m.IsPrivate)
            .Where(m => !Options.MethodsToIgnore.Contains(m.Name));
    }

    /// <summary>Calls all methods to test parameter being set to null.</summary>
    /// <param name="testOrigin">Method under test.</param>
    /// <param name="testParam">Parameter being set to null.</param>
    /// <param name="instance">Instance whose methods to test.</param>
    protected async Task CallAllMethods(
        MethodBase? testOrigin,
        ParameterInfo? testParam,
        object instance
    )
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        foreach (
            MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance)
                .Where(m => !m.IsFamily)
                .Select(m => GenericFixer.FixMethod(m, options))
        )
        {
            MethodCallWrapper data = options.Runner.CreateFor(method, Options.InjectionValues);
            try
            {
                Disposer.Cleanup(
                    await RunCheck(testOrigin ?? method, testParam, instance, data)
                        .ConfigureAwait(false)
                );
            }
            finally
            {
                DisposeAllButInjected(data);
            }
        }
    }

    /// <summary>Runs the check.</summary>
    /// <param name="testOrigin">Method under test.</param>
    /// <param name="testParam">Parameter being set to null.</param>
    /// <param name="data">Call to invoke and test.</param>
    /// <returns>Returned result from the call.</returns>
    protected async Task<object?> RunCheck(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        object? instance,
        MethodCallWrapper data
    )
    {
        ArgumentGuard.ThrowIfNull(testOrigin, nameof(testOrigin));

        RunResult result = await Options.Runner.Run(instance, data).ConfigureAwait(false);
        if (!result.ThrewException)
        {
            return result.Result;
        }
        else
        {
            if (HandleCheckException(testOrigin, testParam, (Exception)result.Result!))
            {
                return null;
            }
            else
            {
                throw (Exception)result.Result!;
            }
        }
    }

    /// <summary>Checks data for disposables and disposes them.</summary>
    /// <param name="data">Data to check and dispose.</param>
    protected void DisposeAllButInjected(object? data)
    {
        if (data is IDictionary asDict)
        {
            DisposeAllButInjected(asDict.Keys);
            DisposeAllButInjected(asDict.Values);
        }
        else if (data is IEnumerable asEnum && asEnum is not string)
        {
            IEnumerator gen = asEnum.GetEnumerator();
            while (gen.MoveNext())
            {
                DisposeAllButInjected(gen.Current);
            }
        }
        else if (!Options.InjectionValues.Any(v => ReferenceEquals(data, v)))
        {
            Disposer.Cleanup(data);
        }
    }

    /// <summary>Handles exceptions encountered by the check.</summary>
    /// <param name="testOrigin">Method under test.</param>
    /// <param name="testParam">Parameter being set to null.</param>
    /// <param name="taskException">Exception encountered.</param>
    /// <returns>If the exception is handled and should not be rethrown.</returns>
    protected abstract bool HandleCheckException(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        Exception taskException
    );
}

#pragma warning restore CA1822 // Member does not access instance data and can be marked static
