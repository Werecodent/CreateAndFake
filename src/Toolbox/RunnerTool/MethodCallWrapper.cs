using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.RunnerTool;

/// <summary>Holds parameter data for a method.</summary>
/// <param name="method"><inheritdoc cref="Method" path="/summary"/></param>
/// <param name="args"><inheritdoc cref="_args" path="/summary"/></param>
public sealed class MethodCallWrapper(MethodBase method, OrderedDictionary args)
    : IDuplicatable<MethodCallWrapper>,
        IValuerAsyncComparable
{
    /// <summary>Parameter names with associated data to pass.</summary>
    private readonly OrderedDictionary _args =
        args ?? throw new ArgumentNullException(nameof(args));

    /// <summary>Associated method.</summary>
    public MethodBase Method { get; } = method ?? throw new ArgumentNullException(nameof(method));

    /// <summary>Parameter data for the method.</summary>
    public IEnumerable<object?> Args => _args.Values.Cast<object>();

    /// <summary>Number of args.</summary>
    public int ArgCount => _args.Count;

    /// <summary>Sets parameter named <paramref name="name"/> to <paramref name="value"/>.</summary>
    /// <param name="name">Name for the parameter to modify.</param>
    /// <param name="value">New value to use.</param>
    /// <returns>Previous value that was replaced for the parameter.</returns>
    /// <exception cref="KeyNotFoundException">If <paramref name="name"/> is not a key.</exception>
    public object? ModifyArg(string name, object? value)
    {
        if (_args.Contains(name))
        {
            object? prev = _args[name];
            _args[name] = value;
            return prev;
        }
        else
        {
            throw new KeyNotFoundException($"Parameter '{name}' not on method '{Method.Name}'.");
        }
    }

    /// <summary>Sets parameter at <paramref name="index"/> to <paramref name="value"/>.</summary>
    /// <param name="index">Index of the parameter to modify.</param>
    /// <param name="value">New value to use.</param>
    /// <returns>Previous value that was replaced for the parameter.</returns>
    public object? ModifyArg(int index, object? value)
    {
        object? prev = _args[index];
        _args[index] = value;
        return prev;
    }

    /// <summary>Invokes the method on <paramref name="instance"/>.</summary>
    /// <param name="instance">Instance to call the method with the data on.</param>
    /// <returns>Results from the call.</returns>
    public object? InvokeOn(object? instance)
    {
        if (instance == null && method is ConstructorInfo builder)
        {
            return builder.Invoke([.. Args]);
        }
        else
        {
            return Method.Invoke(instance, [.. Args]);
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Difference> CompareAsync(
        object? other,
        IValuer valuer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        if (other is MethodCallWrapper wrapper)
        {
            await foreach (
                Difference diff in valuer
                    .CompareAsync(Method, wrapper.Method, canceler)
                    .ConfigureAwait(false)
            )
            {
                canceler.ThrowIfCancellationRequested();
                yield return new Difference(".Method", diff);
            }

            await foreach (
                Difference diff in valuer
                    .CompareAsync(Args, wrapper.Args, canceler)
                    .ConfigureAwait(false)
            )
            {
                canceler.ThrowIfCancellationRequested();
                yield return new Difference(".Arg", diff);
            }
        }
        else
        {
            yield return new Difference(typeof(MethodCallWrapper), other?.GetType());
        }
    }

    /// <inheritdoc/>
    public Task<int> GetValueHashAsync(IValuer valuer, CancellationToken canceler)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCodeAsync(Args.Prepend(Method), canceler);
    }

    /// <inheritdoc/>
    public MethodCallWrapper DeepClone(IDuplicator duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        return new MethodCallWrapper(duplicator.Copy(method), duplicator.Copy(_args));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string nl = Environment.NewLine;

        StringBuilder argDetails = new();
        foreach (DictionaryEntry entry in _args)
        {
            argDetails.Append(nl).Append(" - ").Append(entry.Key).Append(": ");

            if (entry.Value is IDisposable)
            {
                argDetails.Append('*');
            }
            if (entry.Value is IAsyncDisposable)
            {
                argDetails.Append("**");
            }

            if (entry.Value == null)
            {
                argDetails.Append("'NULL'");
            }
            else if (entry.Value is Fake or IFaked)
            {
                argDetails.Append("'FAKE'");
            }
            else if (entry.Value is ICollection series)
            {
                argDetails
                    .Append('(')
                    .Append(series.Count)
                    .Append(") ")
                    .Append(GenericConverter.ExpandName(entry.Value));
            }
            else if (entry.Value.GetType().Inherits(typeof(ICollection<>)))
            {
                argDetails
                    .Append('(')
                    .Append(((dynamic)entry.Value).Count)
                    .Append(") ")
                    .Append(GenericConverter.ExpandName(entry.Value));
            }
            else
            {
                argDetails.Append(entry.Value.ToString());
            }
        }

        return GenericConverter.BuildTestName(Method) + argDetails;
    }
}
