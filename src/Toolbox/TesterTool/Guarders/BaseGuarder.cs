using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.TesterTool.Guarders;

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

        if (kind.HasFlag(BindingFlags.Static))
        {
            methods = methods
                .Concat(
                    Options.IncludeInternals
                        ? describer.StaticMethods.Visible
                        : describer.StaticMethods.OnlyPublic
                )
                .Where(m => !Attribute.IsDefined(m, typeof(CompilerGeneratedAttribute))); // Remove local functions.
        }

        if (kind.HasFlag(BindingFlags.Instance))
        {
            methods = methods.Concat(
                Options.IncludeInternals ? describer.Methods.Visible : describer.Methods.OnlyPublic
            );
        }

        return methods
            .Where(m => !Options.MethodsToIgnore.Contains(m.Name))
            .Where(m => !Options.OnlyDeclaredMethods || m.DeclaringType == type);
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
                opt => opt with { InjectionValues = Options.InjectionValues },
                canceler
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

        if (
            result.ThrewException
            && !HandleCheckException(testOrigin, testParam, (Exception)result.Result!)
        )
        {
            throw new TesterFailureException(
                $"Encountered exception when testing '{data}'.",
                (Exception)result.Result!
            );
        }
        return result.Result;
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
        else if (!Options.InjectionValues.Contains(data))
        {
            await Disposer.CleanupAsync(data).ConfigureAwait(false);
        }
    }

    /// <summary>Checks data for disposables and disposes them.</summary>
    /// <param name="data">Data to check and dispose.</param>
    private async Task DisposeSeriesButInjectedAsync(IEnumerable? data)
    {
        Type? series = GenericConverter.AsConcreteType(data?.GetType(), typeof(IEnumerable<>));
        if (series != null)
        {
            Type arg = series.GetGenericArguments()[0];
            if (!arg.Inherits<IDisposable>() || !arg.Inherits<IAsyncDisposable>())
            {
                return;
            }
        }

        if (data is not null and not string)
        {
            int i = 0;
            foreach (object item in data)
            {
                ArgumentGuard.ThrowUponIterationLimit(i++, Options.Valuer.Options.IterationLimit);

                if (!Options.InjectionValues.Contains(item))
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
