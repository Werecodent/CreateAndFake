using System.Collections.Specialized;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.RunnerTool;

/// <inheritdoc cref="IRunner"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public sealed class Runner(RunnerOptions options) : IRunner
{
    /// <inheritdoc/>
    public RunnerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

#pragma warning disable CA1031 // Do not catch general exception types: Required for testing any exception.
    /// <inheritdoc/>
    public RunResults CallMethodsOn(object instance, RunnerMod? optionConfiguration = null)
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
            MethodCallWrapper data =
                optionConfiguration != null
                    ? CreateFor(method, optionConfiguration)
                    : CreateFor(method);

            object? result;
            try
            {
                result = data.InvokeOn(instance);
            }
            catch (Exception e)
            {
                result = e;
            }
            results.Add(new RunResult(method, data.Args, result));
        }
        return new(results);
    }
#pragma warning restore CA1031 // Do not catch general exception types

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
        Tuple<Type, object> match = data.FirstOrDefault(t =>
            t.Item1.Inherits(param.ParameterType)
        )!;
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
}
