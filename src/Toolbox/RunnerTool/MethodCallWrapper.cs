using System.Collections.Specialized;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool;

namespace CreateAndFake.RunnerTool;

/// <summary>Holds parameter data for a method.</summary>
/// <param name="method"><inheritdoc cref="Method" path="/summary"/></param>
/// <param name="args"><inheritdoc cref="_args" path="/summary"/></param>
public sealed class MethodCallWrapper(MethodBase method, OrderedDictionary args) : IDuplicatable
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
    public IDuplicatable DeepClone(IDuplicator duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        return new MethodCallWrapper(duplicator.Copy(method), duplicator.Copy(_args));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(MethodCallWrapper)}({TypeDescriber.BuildTestName(method)})";
    }
}
