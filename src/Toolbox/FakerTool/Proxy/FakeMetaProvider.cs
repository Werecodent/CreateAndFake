using System.Linq.Expressions;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.DuplicatorTool;

namespace CreateAndFake.FakerTool.Proxy;

#pragma warning disable S2696 // Thread local.

/// <summary>Internal mechanism for faked object behavior.</summary>
/// <param name="identifier"><inheritdoc cref="Identifier" path="/summary"/></param>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
public sealed class FakeMetaProvider(int identifier, FakerOptions options)
    : IDuplicatable<FakeMetaProvider>
{
    /// <summary>Last called method.</summary>
    [ThreadStatic]
    private static Tuple<FakeMetaProvider, CallData>? _LastCall;

    /// <summary>Faked behavior.</summary>
    private readonly Stack<(CallData, Behavior)> _behavior = new();

    /// <summary>Record of calls made.</summary>
    private readonly List<CallData> _log = [];

    /// <summary>Value assigned to identify the instance.</summary>
    /// <remarks>Uniqueness not guaranteed nor verified.</remarks>
    public int Identifier { get; } = identifier;

    /// <summary>Determines behavior when missing set behavior for a call.</summary>
    public bool ThrowByDefault { get; set; } = true;

    /// <inheritdoc cref="FakerOptions"/>
    internal FakerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc cref="FakeMetaProvider"/>
    /// <param name="behavior">Behavior to pass in.</param>
    /// <param name="log">Record of calls to pass in.</param>
    /// <remarks>Copy constructor.</remarks>
    internal FakeMetaProvider(
        int identifier,
        FakerOptions options,
        IEnumerable<(CallData, Behavior)> behavior,
        IEnumerable<CallData> log
    )
        : this(identifier, options)
    {
        ArgumentGuard.ThrowIfNull(behavior, log);

        foreach ((CallData, Behavior) set in behavior)
        {
            _behavior.Push(set);
        }
        _log.AddRange(log);
    }

    /// <inheritdoc/>
    public FakeMetaProvider DeepClone(IDuplicator duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        return new FakeMetaProvider(
            Identifier,
            duplicator.Copy(Options),
            _behavior.Reverse().Select(t => duplicator.Copy(t)),
            _log.Select(t => duplicator.Copy(t))
        )
        {
            ThrowByDefault = ThrowByDefault,
        };
    }

    /// <summary>Sets up behavior for the fake method last called.</summary>
    /// <param name="behavior">Behavior to tie to the call.</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal static void SetLastCallBehavior(Behavior behavior)
    {
        if (_LastCall == null)
        {
            throw new InvalidOperationException(
                "Faked method never called to set behavior for. Verify instance is a [Fake] stub, and method is abstract."
            );
        }

        _ = _LastCall.Item1._log.Remove(_LastCall.Item2);
        _LastCall.Item1.SetCallBehavior(_LastCall.Item2, behavior);
        _LastCall.Item2.ConvertArgs(Arg.CaptureSetArgs());
        _LastCall = null;
    }

    /// <summary>Sets up behavior for the fake.</summary>
    /// <param name="callData">Call to set behavior for.</param>
    /// <param name="behavior">Behavior to tie to the call.</param>
    internal void SetCallBehavior(CallData callData, Behavior behavior)
    {
        ArgumentGuard.ThrowIfNull(callData, behavior);

        _behavior.Push((callData, behavior));
    }

    /// <summary>Verifies behavior with associated times were called as expected.</summary>
    /// <exception cref="FakeVerifyException"></exception>
    internal void Verify()
    {
        (CallData, Behavior)[] invalids = [.. _behavior.Where(t => !t.Item2.HasExpectedCalls())];
        if (invalids.Length != 0)
        {
            throw new FakeVerifyException(invalids, _log);
        }
    }

    /// <summary>Verifies the number of calls made.</summary>
    /// <param name="times">Expected number of calls.</param>
    /// <param name="callData">Call to verify.</param>
    /// <exception cref="FakeVerifyException"></exception>
    internal void Verify(Times times, CallData callData)
    {
        ArgumentGuard.ThrowIfNull(times, callData);

        IEnumerable<CallData> calls = [.. _log.Where(callData.MatchesCall)];
        if (!times.IsInRange(calls.Count()))
        {
            throw new FakeVerifyException(callData, times, calls.Count(), _log);
        }
    }

    /// <summary>Verifies the total number of calls made.</summary>
    /// <param name="times">Expected total.</param>
    /// <exception cref="FakeVerifyException"></exception>
    internal void VerifyTotalCalls(Times times)
    {
        ArgumentGuard.ThrowIfNull(times);

        if (!times.IsInRange(_log.Count))
        {
            throw new FakeVerifyException(times, _log);
        }
    }

    /// <summary>Manager for all action calls.</summary>
    /// <param name="instance">The faked object.</param>
    /// <param name="calledMethod">Method being called.</param>
    /// <param name="generics">Generics tied to the call.</param>
    /// <param name="args">Provided args to the call.</param>
    /// <exception cref="InvalidOperationException"></exception>
    internal void CallVoid(object instance, MethodInfo calledMethod, Type[] generics, object[] args)
    {
        object? result = CallBehavior<object>(instance, calledMethod, generics, args, true);
        if (result != null)
        {
            throw new InvalidOperationException(
                $"Method '{GetMethodName(calledMethod)}' expected void but instead returned '{result}'."
            );
        }
    }

    /// <summary>Manager for all func calls.</summary>
    /// <typeparam name="T">Return type.</typeparam>
    /// <param name="instance">The faked object.</param>
    /// <param name="calledMethod">Method being called.</param>
    /// <param name="generics">Generics tied to the call.</param>
    /// <param name="args">Provided args to the call.</param>
    /// <returns>Faked result previously set up.</returns>
    /// <exception cref="FakeCallException"></exception>
    internal T? CallRet<T>(object instance, MethodInfo calledMethod, Type[] generics, object[] args)
    {
        return CallBehavior<T>(instance, calledMethod, generics, args, false);
    }

    /// <summary>Manager for all calls.</summary>
    /// <typeparam name="T">Return type.</typeparam>
    /// <param name="instance">The faked object.</param>
    /// <param name="calledMethod">Method being called.</param>
    /// <param name="generics">Generics tied to the call.</param>
    /// <param name="args">Provided args to the call.</param>
    /// <param name="isVoid">If method called is void.</param>
    /// <returns>Faked result previously set up.</returns>
    /// <exception cref="FakeCallException"></exception>
    private T? CallBehavior<T>(
        object instance,
        MethodInfo calledMethod,
        Type[] generics,
        object[] args,
        bool isVoid
    )
    {
        ArgumentGuard.ThrowIfNull(calledMethod);

        CallData data = new(GetMethodName(calledMethod), generics, args, Options);
        _log.Add(data);
        _LastCall = Tuple.Create(this, data);

        (CallData, Behavior) match = _behavior.FirstOrDefault(t => t.Item1.MatchesCall(data));
        if (match.Equals(default))
        {
            if (ThrowByDefault && GetMethodName(calledMethod) != "Finalizer")
            {
                throw new FakeCallException(data, _behavior.Select(b => b.Item1));
            }
            else if (!isVoid && Options.FakeDefaultGenerator != null)
            {
                return (T?)
                    Options
                        .FakeDefaultGenerator.Invoke(
                            calledMethod.IsGenericMethod
                                ? calledMethod.MakeGenericMethod(generics)
                                : calledMethod
                        )
                        .Invoke(args);
            }
            else
            {
                return default;
            }
        }

        if (match.Item2.CallBase)
        {
            return CallBase<T>(instance, calledMethod, generics, match, args);
        }
        else
        {
            return (T?)match.Item2.Invoke(args);
        }
    }

    /// <summary>Calls the base method for a behavior.</summary>
    /// <typeparam name="T">Return type.</typeparam>
    /// <param name="instance">The faked object.</param>
    /// <param name="calledMethod">Method being called.</param>
    /// <param name="generics">Generics tied to the call.</param>
    /// <param name="match">Behavior details.</param>
    /// <param name="args">Provided args to the call.</param>
    /// <returns>Base method result.</returns>
    /// <exception cref="MissingMethodException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static T? CallBase<T>(
        object instance,
        MethodInfo calledMethod,
        Type[] generics,
        (CallData, Behavior) match,
        object[] args
    )
    {
        string specificName = GetMethodName(calledMethod);
        Type[] specificParameters =
        [
            .. (
                calledMethod.IsGenericMethod
                    ? calledMethod.MakeGenericMethod(generics)
                    : calledMethod
            )
                .GetParameters()
                .Select(p => p.ParameterType),
        ];

        MethodInfo? searchForSourceMethod()
        {
            Type? currentType = calledMethod.DeclaringType?.BaseType;
            while (currentType != null)
            {
                MethodInfo? method = TypeDescriber
                    .For(currentType)
                    .Methods.All.Where(m => m.Name == specificName)
                    .Where(m =>
                        generics.Length == (m.IsGenericMethod ? m.GetGenericArguments().Length : 0)
                    )
                    .Select(m => m.IsGenericMethod ? m.MakeGenericMethod(generics) : m)
                    .FirstOrDefault(m =>
                        m.GetParameters()
                            .Select(p => p.ParameterType)
                            .SequenceEqual(specificParameters)
                    );

                if (method != null)
                {
                    return method;
                }

                currentType = currentType.BaseType;
            }
            return null;
        }

        MethodInfo? found = searchForSourceMethod();

        if (found == null)
        {
            throw new MissingMethodException(
                $"Method '{GetMethodName(calledMethod)}' does not exist on '{calledMethod.DeclaringType?.BaseType}'"
            );
        }
        else if (found.IsAbstract)
        {
            throw new InvalidOperationException(
                $"Cannot call base '{GetMethodName(calledMethod)}' as it's abstract."
            );
        }
        else
        {
            Delegate caller = (Delegate)
                Activator.CreateInstance(
                    FindDelegateType(found),
                    instance,
                    found.MethodHandle.GetFunctionPointer()
                )!;

            return (T?)match.Item2.Invoke(caller, args);
        }
    }

    private static string GetMethodName(MethodInfo method)
    {
        return method.Name.Substring(method.Name.LastIndexOf('.') + 1);
    }

    /// <summary>Matches a delegate to the method.</summary>
    /// <param name="methodInfo">Method to call.</param>
    /// <returns>Found delegate type.</returns>
    private static Type FindDelegateType(MethodInfo methodInfo)
    {
        IEnumerable<Type> args = methodInfo.GetParameters().Select(p => p.ParameterType);

        return (methodInfo.ReturnType == typeof(void))
            ? Expression.GetActionType([.. args])
            : Expression.GetFuncType([.. args, methodInfo.ReturnType]);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{GenericConverter.ExpandName(GetType())}({Identifier})";
    }
}

#pragma warning restore
