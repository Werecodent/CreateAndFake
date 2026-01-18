using System.Collections;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RunnerTool;

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
        ArgumentGuard.ThrowIfNull(type);

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
        ArgumentGuard.ThrowIfNull(type);

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
        ArgumentGuard.ThrowIfNull(instance);

        foreach (
            MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance)
                .Where(m => !m.IsFamily)
                .Select(m => GenericFixer.FixMethod(m, options))
        )
        {
            MethodCallWrapper data = options.Runner.CreateFor(method, Options.InjectionValues);
            try
            {
                await Disposer
                    .CleanupAsync(
                        await RunCheck(testOrigin ?? method, testParam, instance, data)
                            .ConfigureAwait(false)
                    )
                    .ConfigureAwait(false);
            }
            finally
            {
                await DisposeAllButInjected(data).ConfigureAwait(false);
            }
        }
    }

    /// <summary>Runs the check.</summary>
    /// <param name="testOrigin">Method under test.</param>
    /// <param name="testParam">Parameter being set to null.</param>
    /// <param name="instance"></param>
    /// <param name="data">Call to invoke and test.</param>
    /// <returns>Returned result from the call.</returns>
    protected async Task<object?> RunCheck(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        object? instance,
        MethodCallWrapper data
    )
    {
        ArgumentGuard.ThrowIfNull(testOrigin);

        RunResult result = await Options.Runner.Run(instance, data).ConfigureAwait(false);
        if (!result.ThrewException)
        {
            return result.Result;
        }
        else if (HandleCheckException(testOrigin, testParam, (Exception)result.Result!))
        {
            return null;
        }
        else
        {
            throw (Exception)result.Result!;
        }
    }

    /// <summary>Checks data for disposables and disposes them.</summary>
    /// <param name="data">Data to check and dispose.</param>
    protected async Task DisposeAllButInjected(object? data)
    {
        if (data is IDictionary asDict)
        {
            await DisposeAllButInjected(asDict.Keys).ConfigureAwait(false);
            await DisposeAllButInjected(asDict.Values).ConfigureAwait(false);
        }
        else if (data is IEnumerable asEnum)
        {
            await DisposeAllButInjected(asEnum).ConfigureAwait(false);
        }
        else if (!Options.InjectionValues.Any(v => ReferenceEquals(data, v)))
        {
            await Disposer.CleanupAsync(data).ConfigureAwait(false);
        }
    }

    /// <summary>Checks data for disposables and disposes them.</summary>
    /// <param name="data">Data to check and dispose.</param>
    protected async Task DisposeAllButInjected(IEnumerable? data)
    {
        if (data is not null and not string)
        {
            foreach (object item in data)
            {
                if (!Options.InjectionValues.Any(v => ReferenceEquals(item, v)))
                {
                    await Disposer.CleanupAsync(item).ConfigureAwait(false);
                }
            }
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
