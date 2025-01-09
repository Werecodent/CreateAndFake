using System.Collections.Specialized;
using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.RandomizerTool;

/// <summary>Holds parameter data for a method.</summary>
public sealed class MethodCallWrapper
{
    /// <summary>Associated method.</summary>
    private readonly MethodBase _method;

    /// <summary>Parameter names with associated data to pass.</summary>
    private readonly OrderedDictionary _args;

    /// <summary>Parameter data for the method.</summary>
    public IEnumerable<object?> Args => _args.Values.Cast<object>();

    /// <inheritdoc cref="MethodCallWrapper"/>
    /// <param name="method"><inheritdoc cref="_method" path="/summary"/></param>
    /// <param name="args"><inheritdoc cref="_args" path="/summary"/></param>
    public MethodCallWrapper(MethodBase method, OrderedDictionary args)
    {
        ArgumentGuard.ThrowIfNull(args, nameof(args));

        _method = method ?? throw new ArgumentNullException(nameof(method));
        _args = args ?? throw new ArgumentNullException(nameof(method));
    }

    /// <summary>Sets parameter named <paramref name="name"/> to <paramref name="value"/>.</summary>
    /// <param name="name">Name for the parameter to modify.</param>
    /// <param name="value">New value to use.</param>
    public void ModifyArg(string name, object value)
    {
        if (_args.Contains(name))
        {
            _args[name] = value;
        }
        else
        {
            throw new KeyNotFoundException($"Parameter '{name}' not on method '{_method.Name}'.");
        }
    }

    /// <summary>Invokes the method on <paramref name="instance"/>.</summary>
    /// <param name="instance">Instance to call the method with the data on.</param>
    /// <returns>Results from the call.</returns>
    public object? InvokeOn(object instance)
    {
        return _method.Invoke(instance, _args.Values.Cast<object>().ToArray());
    }
}
