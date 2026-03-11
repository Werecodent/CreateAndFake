using System.Collections.Specialized;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.RunnerTool;

/// <inheritdoc cref="IRunner"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Runner(RunnerOptions options) : IRunner
{
    /// <inheritdoc/>
    public RunnerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public async Task<RunResults> CallMethodsOnAsync(
        object instance,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance);

        RunnerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        List<RunResult> results = [];
        foreach (
            MethodInfo method in instance
                .GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.DeclaringType != typeof(object))
        )
        {
            // Sequentially executed to prevent concurrency issues; do not attempt to parallelize.
            results.Add(
                await RunAsync(
                        instance,
                        GenericResolver.OfConcrete(method, localOptions.Randomizer),
                        canceler,
                        (optionConfiguration != null) ? _ => localOptions : null
                    )
                    .ConfigureAwait(false)
            );
        }
        return new(results);
    }

    /// <inheritdoc/>
    public Task<RunResult> RunAsync(
        object? instance,
        MethodInfo method,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    )
    {
        MethodCallWrapper data = CreateFor(method, optionConfiguration, canceler);

        return RunAsync(instance, data, canceler, optionConfiguration);
    }

    /// <inheritdoc/>
    public async Task<RunResult> RunAsync(
        object? instance,
        MethodCallWrapper data,
        CancellationToken canceler,
        RunnerMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(data);

        RunnerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        TimeSpan timeout =
            (localOptions.Timeout.TotalMilliseconds is >= -1)
                ? localOptions.Timeout
                : TimeSpan.FromMilliseconds(30000);

        Task<object?> task;
        using (
            CancellationTokenSource timeoutTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(canceler)
        )
        {
            task = Task.Run(
                async () =>
                    await Unwrapper
                        .UnwrapResult(() => data.InvokeOn(instance), localOptions)
                        .ConfigureAwait(false),
                timeoutTokenSource.Token
            );

            bool timedOut =
                (
                    await Task.WhenAny(task, Task.Delay(timeout, timeoutTokenSource.Token))
                        .ConfigureAwait(false)
                ) != task;

            await AsyncSeriesHelper
                .TriggerCancellationAsync(timeoutTokenSource)
                .ConfigureAwait(false);

            if (timedOut)
            {
                throw new TimeoutException(
                    $"Attempting to run method '{data.Method.Name}' timed out: {timeout}"
                );
            }
        }

        if (task.Exception != null)
        {
            return new(data.Method, data.Args, UnwrapException(task.Exception), true);
        }

        try
        {
            object? result = await task.ConfigureAwait(false);
            return new(data.Method, data.Args, result, false);
        }
        catch (Exception taskException)
        {
            return new(data.Method, data.Args, UnwrapException(taskException), true);
        }
    }

    private static Exception? UnwrapException(Exception? error)
    {
        Exception? result = error;
        if (result is AggregateException multi && multi.InnerExceptions.Count == 1)
        {
            result = multi.InnerException;
        }

        if (result is TargetInvocationException ex)
        {
            result = ex.InnerException;
        }

        return result;
    }

    /// <inheritdoc/>
    public MethodCallWrapper CreateFor(
        MethodBase method,
        CancellationToken canceler,
        params IEnumerable<object?>? values
    )
    {
        return CreateFor(method, opt => opt, canceler, values);
    }

    /// <inheritdoc/>
    public MethodCallWrapper CreateFor(
        MethodBase method,
        RunnerMod? optionConfiguration,
        CancellationToken canceler,
        params IEnumerable<object?>? values
    )
    {
        ArgumentGuard.ThrowIfNull(method);

        if (method.IsGenericMethodDefinition)
        {
            throw new UnsupportedException(
                $"Method '{TypeHelper.BuildTestName(method)}' must have "
                    + "generics specified before data can be populated for it."
            );
        }

        RunnerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        List<Tuple<Type, object>> data =
        [
            .. (values ?? [])
                .Where(v => v != null)
                .Select(v => (v is Fake fake) ? fake.Dummy : v)
                .Where(v => v != null)
                .Select(v => Tuple.Create(v!.GetType(), v)),
        ];

        OrderedDictionary args = new(method.GetParameters().Length);

        foreach (ParameterInfo param in method.GetParameters())
        {
            args.Add(
                param.Name ?? $"{args.Count}",
                ExtractArg(param, data, args, localOptions, canceler)
            );
        }

        return new MethodCallWrapper(method, args);
    }

    /// <summary>Randomizes an instance to fill a parameter.</summary>
    /// <param name="param">Parameter to fill.</param>
    /// <param name="data">Canned data to prefer.</param>
    /// <param name="args">Already created parameter data.</param>
    /// <param name="localOptions">Potentially modified configuration to use.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The created arg to fill the parameter with.</returns>
    private static object? ExtractArg(
        ParameterInfo param,
        List<Tuple<Type, object>> data,
        OrderedDictionary args,
        RunnerOptions localOptions,
        CancellationToken canceler
    )
    {
        Tuple<Type, object> match = data.Find(t => t.Item1.Inherits(param.ParameterType))!;
        if (param.IsOut)
        {
            return null;
        }
        else if (param.GetCustomAttributes<BaseFakeAttribute>().Any())
        {
            return (
                (Fake)
                    localOptions.Randomizer.Create(
                        typeof(Fake<>).MakeGenericType(param.ParameterType)
                    )!
            ).Dummy;
        }
        else if (param.GetCustomAttributes<BaseStubAttribute>().Any())
        {
            if (
                localOptions.InheritIReflectableTypeOnFakedType
                && param.ParameterType.Inherits<Type>()
            )
            {
                return localOptions.Faker.Stub(param.ParameterType, typeof(IReflectableType)).Dummy;
            }
            else
            {
                return localOptions.Faker.Stub(param.ParameterType).Dummy;
            }
        }
        else if (param.GetCustomAttributes<BaseSizeAttribute>().Any())
        {
            int size = param.GetCustomAttribute<BaseSizeAttribute>()!.Count;
            return localOptions.Randomizer.Create(
                param.ParameterType,
                opt =>
                    opt with
                    {
                        CollectionMinSize = size,
                        CollectionMaxSize = size,
                        StringMinSize = size,
                        StringMaxSize = size,
                        NestedOptions = opt,
                    }
            );
        }
        else if (param.ParameterType == typeof(CancellationToken))
        {
            return canceler;
        }
        else if (match != default)
        {
            _ = data.Remove(match);
            return match.Item2;
        }
        else if (param.ParameterType == typeof(string))
        {
            string? smartData = new DataRandom(localOptions.Gen).Find(param.Name);
            if (smartData != null)
            {
                return smartData;
            }
        }

        return localOptions.Randomizer.Inject(
            param.ParameterType,
            [.. args.Values.Cast<object>().Where(a => a is Fake or IFaked).Reverse()]
        );
    }

    /// <inheritdoc/>
    public IRunner WithOptions(RunnerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Runner(optionConfiguration.Invoke(Options));
    }
}
