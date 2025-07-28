using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
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
    public async Task<RunResults> CallMethodsOn(
        object instance,
        RunnerMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(instance, nameof(instance));

        List<RunResult> results = [];
        foreach (
            MethodInfo method in instance
                .GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.DeclaringType != typeof(object))
        )
        {
            // Sequentially executed to prevent concurrency issues; do not attempt to parallelize.
            results.Add(await Run(instance, method, optionConfiguration).ConfigureAwait(false));
        }
        return new(results);
    }

    /// <summary>Ensures the result is completed.</summary>
    /// <param name="call">Potentially wrapped data.</param>
    /// <returns>The unwrapped result.</returns>
    private static async Task<(bool, object?)> UnwrapTaskResult(Func<object?> call)
    {
        object? result = call.Invoke();

        if (result?.GetType().Inherits(typeof(IAsyncEnumerable<>)) ?? false)
        {
            result = typeof(Runner)
                .GetMethod(nameof(EnumerateAsync), BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(
                    TypeDescriber
                        .FindConcreteInterface(result.GetType(), typeof(IAsyncEnumerable<>))
                        .GetGenericArguments()
                )
                .Invoke(null, [result]);
        }

        if (result == null)
        {
            return (true, null);
        }

        if (result is ValueTask valueTask)
        {
            if (result.GetType().Inherits(typeof(ValueTask<>)))
            {
                result = typeof(Runner)
                    .GetMethod(
                        nameof(ExecuteValueTask),
                        BindingFlags.Static | BindingFlags.NonPublic
                    )!
                    .MakeGenericMethod(
                        TypeDescriber
                            .FindConcreteInterface(result.GetType(), typeof(ValueTask<>))
                            .GetGenericArguments()
                    )
                    .Invoke(null, [result])!;
            }
            else
            {
                await valueTask.ConfigureAwait(false);
            }
        }

        if (result is Task task)
        {
            await task.ConfigureAwait(false);

            PropertyInfo? prop = result.GetType().GetProperty("Result");
            return (prop != null) ? (true, prop.GetValue(result)) : (false, null);
        }

        Type resultType = result.GetType();
        if (
            resultType.Inherits<ICollection>()
            || resultType.Inherits(typeof(ICollection<>))
            || resultType == typeof(string)
        )
        {
            return (true, result);
        }

        // Required to execute yield return methods.
        if (resultType.Inherits(typeof(IEnumerable<>)))
        {
            return (
                true,
                typeof(Runner)
                    .GetMethod(nameof(Enumerate), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(
                        TypeDescriber
                            .FindConcreteInterface(result.GetType(), typeof(IEnumerable<>))
                            .GetGenericArguments()
                    )
                    .Invoke(null, [result])
            );
        }

        return (true, result);
    }

    private static async Task<T> ExecuteValueTask<T>(ValueTask<T> task)
    {
        return await task.ConfigureAwait(false);
    }

    private static T[] Enumerate<T>(object syncData)
    {
        List<T> results = [];
        results.AddRange((IEnumerable<T>)syncData);
        return [.. results];
    }

    private static async Task<T[]> EnumerateAsync<T>(object asyncData)
    {
        List<T> results = [];
        await foreach (T item in ((IAsyncEnumerable<T>)asyncData).ConfigureAwait(false))
        {
            results.Add(item);
        }
        return [.. results];
    }

    /// <inheritdoc/>
    public Task<RunResult> Run(
        object? instance,
        MethodInfo method,
        RunnerMod? optionConfiguration = null
    )
    {
        MethodCallWrapper data =
            optionConfiguration != null
                ? CreateFor(method, optionConfiguration)
                : CreateFor(method);

        return Run(instance, data, optionConfiguration);
    }

    /// <inheritdoc/>
    public async Task<RunResult> Run(
        object? instance,
        MethodCallWrapper data,
        RunnerMod? optionConfiguration = null
    )
    {
        ArgumentGuard.ThrowIfNull(data, nameof(data));

        RunnerOptions localOptions = optionConfiguration?.Invoke(Options) ?? Options;

        TimeSpan timeout =
            (localOptions.Timeout.TotalMilliseconds is >= -1 and <= 10000) //int.MaxValue)
                ? localOptions.Timeout
                : TimeSpan.FromMilliseconds(10000);

        Task<(bool, object?)> task = Task.Run(() =>
            UnwrapTaskResult(() => data.InvokeOn(instance))
        );

        using (CancellationTokenSource stopper = new())
        {
            if (
                (await Task.WhenAny(task, Task.Delay(timeout, stopper.Token)).ConfigureAwait(false))
                != task
            )
            {
                throw new TimeoutException(
                    $"Attempting to run method '{data.Method.Name}' timed out."
                );
            }
#if LEGACY
            stopper.Cancel();
#else
            await stopper.CancelAsync().ConfigureAwait(false);
#endif
        }

        try
        {
            (bool, object?) result = await task.ConfigureAwait(false);
            return new(data.Method, data.Args, result.Item2, result.Item1, false);
        }
        catch (Exception taskException)
        {
            return new(data.Method, data.Args, UnwrapException(taskException), false, true);
        }
    }

    private static Exception? UnwrapException(Exception? error)
    {
        if (error is AggregateException multi && multi.InnerExceptions.Count == 1)
        {
            return UnwrapException(multi.InnerException);
        }

        if (error is TargetInvocationException ex)
        {
            return UnwrapException(ex.InnerException);
        }

        return error;
    }

    /// <inheritdoc/>
    public MethodCallWrapper CreateFor(MethodBase method, params IEnumerable<object?>? values)
    {
        return CreateFor(method, opt => opt, values);
    }

    /// <inheritdoc/>
    public MethodCallWrapper CreateFor(
        MethodBase method,
        RunnerMod optionConfiguration,
        params IEnumerable<object?>? values
    )
    {
        ArgumentGuard.ThrowIfNull(method, nameof(method));

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
            args.Add(param.Name ?? $"{args.Count}", ExtractArg(param, data, args, localOptions));
        }

        return new MethodCallWrapper(method, args);
    }

    /// <summary>Randomizes an instance to fill a parameter.</summary>
    /// <param name="param">Parameter to fill.</param>
    /// <param name="data">Canned data to prefer.</param>
    /// <param name="args">Already created parameter data.</param>
    /// <param name="localOptions">Potentially modified configuration to use.</param>
    /// <returns>The created arg to fill the parameter with.</returns>
    private static object? ExtractArg(
        ParameterInfo param,
        List<Tuple<Type, object>> data,
        OrderedDictionary args,
        RunnerOptions localOptions
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
        else if (match != default)
        {
            _ = data.Remove(match);
            return match.Item2;
        }
        else
        {
            return localOptions.Randomizer.Inject(
                param.ParameterType,
                [.. args.Values.Cast<object>().Where(a => a is Fake or IFaked).Reverse()]
            );
        }
    }

    /// <inheritdoc/>
    public IRunner WithOptions(RunnerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new Runner(optionConfiguration.Invoke(Options));
    }
}
