using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.FakerTool.Proxy;

/// <summary>Method call details.</summary>
/// <param name="methodName"><inheritdoc cref="_methodName" path="/summary"/></param>
/// <param name="generics"><inheritdoc cref="_generics" path="/summary"/></param>
/// <param name="args"><inheritdoc cref="_args" path="/summary"/></param>
/// <param name="options"><inheritdoc cref="_options" path="/summary"/></param>
internal sealed class CallData(string methodName, Type[] generics, object?[] args, FakerOptions? options)
{
    /// <summary>Name tied to the call.</summary>
    private readonly string _methodName = methodName ?? throw new ArgumentNullException(nameof(methodName));

    /// <summary>Generics tied to the call.</summary>
    private readonly Type[] _generics = generics ?? throw new ArgumentNullException(nameof(generics));

    /// <summary>Args tied to the call.</summary>
    private readonly object?[] _args = args ?? throw new ArgumentNullException(nameof(args));

    /// <summary>How to compare call data.</summary>
    private readonly FakerOptions? _options = options;

    /// <summary>Converts arg values to designated Arg matchers.</summary>
    /// <param name="argChanges">Created Args to substitute on the given values.</param>
    internal void ConvertArgs(Tuple<Arg, object>[] argChanges)
    {
        ArgumentGuard.ThrowIfNull(argChanges, nameof(argChanges));

        List<Tuple<Arg, object>> changes = [.. argChanges];
        for (int i = 0; i < _args.Length; i++)
        {
            Tuple<Arg, object>? change = changes.FirstOrDefault(c => Equals(c.Item2, _args[i]));
            if (change != default)
            {
                _args[i] = change.Item1;
                _ = changes.Remove(change);
            }
        }
    }

    /// <summary>Determines if behavior is intended for a call.</summary>
    /// <param name="input">Details of the call.</param>
    /// <returns><c>true</c> if matched; <c>false</c> otherwise.</returns>
    internal bool MatchesCall(CallData input)
    {
        ArgumentGuard.ThrowIfNull(input, nameof(input));

        return (_methodName == input._methodName)
            && GenericsMatch(input._generics)
            && ArgsMatch(input._args);
    }

    /// <summary>Determines if the call generics match the expected ones.</summary>
    /// <param name="inputGenerics">Generics used in the call.</param>
    /// <returns><c>true</c> if matched; <c>false</c> otherwise.</returns>
    private bool GenericsMatch(Type[] inputGenerics)
    {
        bool matches = inputGenerics.Length == _generics.Length;

        for (int i = 0; matches && i < inputGenerics.Length; i++)
        {
            if (_generics[i] != typeof(AnyGeneric))
            {
                matches &= _generics[i] == inputGenerics[i];
            }
        }
        return matches;
    }

    /// <summary>Determines if the call args match the expected ones.</summary>
    /// <param name="inputArgs">Args used in the call.</param>
    /// <returns><c>true</c> if matched; <c>false</c> otherwise.</returns>
    private bool ArgsMatch(object?[] inputArgs)
    {
        bool matches = inputArgs.Length == _args.Length;

        for (int i = 0; matches && i < inputArgs.Length; i++)
        {
            if (_args[i] is Arg exp)
            {
                matches &= exp.Matches(inputArgs[i]);
            }
            else if (_options?.Valuer != null)
            {
                matches &= _options.Valuer.Equals(inputArgs[i], _args[i]);
            }
            else
            {
                matches &= Equals(inputArgs[i], _args[i]);
            }
        }
        return matches;
    }

    /// <summary>Converts <c>this</c> to a <c>string</c>.</summary>
    /// <returns><c>string</c> representation of <c>this</c>.</returns>
    public override string ToString()
    {
        string gen = _generics.Length != 0
            ? "<" + string.Join(", ", _generics.Select(g => g.Name)) + ">"
            : "";
        return _methodName + gen + "(" + string.Join(", ", _args.Select(i => i ?? "'null'")) + ")";
    }
}
