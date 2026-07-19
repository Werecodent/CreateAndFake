using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.TesterTool.Guarders;

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

        return Options.IncludeInternals
            ? TypeDescriber.For(type).Constructors.Visible
            : TypeDescriber.For(type).Constructors.OnlyPublic;
    }

    /// <summary>Gets all testable methods on a type.</summary>
    /// <param name="type">Type with the methods to test.</param>
    /// <param name="kind">Instance, static, or both.</param>
    /// <returns>Found methods.</returns>
    protected IEnumerable<MethodInfo> FindAllMethods(Type type, BindingFlags kind)
    {
        ArgumentGuard.ThrowIfNull(type);

        TypeDescriber describer = TypeDescriber.For(type);
        IEnumerable<MethodInfo> methods = [];

        if (kind.HasFlag(BindingFlags.Instance))
        {
            methods = methods.Concat(
                Options.IncludeInternals ? describer.Methods.Visible : describer.Methods.OnlyPublic
            );
        }

        if (kind.HasFlag(BindingFlags.Static))
        {
            methods = methods.Concat(
                Options.IncludeInternals
                    ? describer.StaticMethods.Visible
                    : describer.StaticMethods.OnlyPublic
            );
        }

        return methods
            .Where(m => m.DeclaringType == type || m.DeclaringType!.IsAbstract)
            .Where(m => !Attribute.IsDefined(m, typeof(CompilerGeneratedAttribute))) // Remove local functions.
            .Where(m => !Options.MethodsToIgnore.Contains(m.Name));
    }

    /// <summary>Attempts to test all methods.</summary>
    /// <param name="type">Type being tested.</param>
    /// <param name="checker">Test to run.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    protected async Task CreateInstanceAndTestMethodsAsync(
        Type type,
        Func<object, CancellationToken, Task> checker,
        CancellationToken canceler
    )
    {
        if (Options.IncludeInstanceMethods && !(type.IsAbstract && type.IsSealed))
        {
            object instance =
                (Options.InjectionValues.Length > 0)
                    ? Options.Randomizer.Inject(type, Options.InjectionValues)
                    : Options.Randomizer.Create(type);
            try
            {
                await checker.Invoke(instance, canceler).ConfigureAwait(false);
            }
            finally
            {
                await Disposer.CleanupAsync(instance).ConfigureAwait(false);
            }
        }
    }

    /// <summary>Calls all methods to test parameter being set to null.</summary>
    /// <param name="testOrigin">Method under test.</param>
    /// <param name="testParam">Parameter being set to null.</param>
    /// <param name="instance">Instance whose methods to test.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    protected async Task CallAllMethodsAsync(
        MethodBase? testOrigin,
        ParameterInfo? testParam,
        object instance,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(instance);

        foreach (
            MethodInfo method in FindAllMethods(instance.GetType(), BindingFlags.Instance)
                .Select(m => GenericFixer.FixMethod(m, options))
        )
        {
            MethodCallWrapper data = options.Runner.CreateFor(
                method,
                canceler,
                Options.InjectionValues
            );
            try
            {
                await Disposer
                    .CleanupAsync(
                        await RunCheckAsync(
                                testOrigin ?? method,
                                testParam,
                                instance,
                                data,
                                canceler
                            )
                            .ConfigureAwait(false)
                    )
                    .ConfigureAwait(false);
            }
            finally
            {
                await DisposeAllButInjectedAsync(data.Args).ConfigureAwait(false);
            }
        }
    }

    /// <summary>Runs the check.</summary>
    /// <param name="testOrigin">Method under test.</param>
    /// <param name="testParam">Parameter being set to null.</param>
    /// <param name="instance"></param>
    /// <param name="data">Call to invoke and test.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Returned result from the call.</returns>
    /// <exception cref="TesterFailureException">If running <paramref name="data"/> created an exception.</exception>
    protected async Task<object?> RunCheckAsync(
        MethodBase testOrigin,
        ParameterInfo? testParam,
        object? instance,
        MethodCallWrapper data,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(testOrigin);

        RunResult result = await Options
            .Runner.RunAsync(instance, data, canceler)
            .ConfigureAwait(false);
        if (!result.ThrewException)
        {
            return result.Result;
        }
        else if (!HandleCheckException(testOrigin, testParam, (Exception)result.Result!))
        {
            throw new TesterFailureException(
                $"Encountered exception when testing '{data}'.",
                (Exception)result.Result!
            );
            // ExceptionDispatchInfo.Capture((Exception)result.Result!).Throw();
        }
        return null;
    }

    /// <summary>Checks data for disposables and disposes them.</summary>
    /// <param name="data">Data to check and dispose.</param>
    protected async Task DisposeAllButInjectedAsync(object? data)
    {
        if (data is IDictionary asDict)
        {
            await DisposeSeriesButInjectedAsync(asDict.Keys).ConfigureAwait(false);
            await DisposeSeriesButInjectedAsync(asDict.Values).ConfigureAwait(false);
        }
        else if (data is IEnumerable asEnum)
        {
            await DisposeSeriesButInjectedAsync(asEnum).ConfigureAwait(false);
        }
        else if (!Options.InjectionValues.Any(v => ReferenceEquals(data, v)))
        {
            await Disposer.CleanupAsync(data).ConfigureAwait(false);
        }
    }

    /// <summary>Checks data for disposables and disposes them.</summary>
    /// <param name="data">Data to check and dispose.</param>
    private async Task DisposeSeriesButInjectedAsync(IEnumerable? data)
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
