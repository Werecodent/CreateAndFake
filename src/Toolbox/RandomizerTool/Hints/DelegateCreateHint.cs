using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

#pragma warning disable S1144 // Used via reflection.

/// <summary>Handles randomizing <see cref="Delegate"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class DelegateCreateHint : CreateHint
{
    /// <summary>Methods used to match delegates.</summary>
    private static readonly ImmutableArray<MethodInfo> _Delegators =
    [
        .. typeof(Delegator)
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(m => m.Name == "AutoDelegate"),
    ];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type == typeof(Delegate))
        {
            return new(Create(typeof(Action), randomizer));
        }
        else if (type.Inherits<Delegate>())
        {
            return new(Create(type, randomizer));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <returns>The randomized instance.</returns>
    /// <inheritdoc cref="CreateHint.TryCreate"/>
    private static object Create(Type type, RandomizerChainer randomizer)
    {
        Delegator delegator = new(randomizer);

        MethodInfo info = type.GetMethod("Invoke")!;
        bool hasReturn = info.ReturnType != typeof(void);

        MethodInfo match = _Delegators
            .Where(m => m.GetParameters().Length == info.GetParameters().Length)
            .Single(m => m.ReturnType != typeof(void) == hasReturn);

        IEnumerable<Type> generics = info.GetParameters().Select(p => p.ParameterType);
        if (!match.IsGenericMethod)
        {
            return Delegate.CreateDelegate(type, delegator, match);
        }
        else if (hasReturn)
        {
            return Delegate.CreateDelegate(
                type,
                delegator,
                match.MakeGenericMethod([.. generics, info.ReturnType])
            );
        }
        else
        {
            return Delegate.CreateDelegate(type, delegator, match.MakeGenericMethod([.. generics]));
        }
    }

    /// <summary>Provides delegate implementations.</summary>
    private sealed class Delegator
    {
        /// <summary>Creates random results.</summary>
        private readonly RandomizerChainer _randomizer;

        /// <summary>Sets up the randomization.</summary>
        /// <param name="randomizer"><inheritdoc cref="_randomizer" path="/summary"/></param>
        internal Delegator(RandomizerChainer randomizer)
        {
            _randomizer = randomizer;
        }

        /// <summary>Randomizes any <see cref="OutRef{T}"/> inputs.</summary>
        /// <param name="inputs">Inputs for the delegate.</param>
        private void HandleIn(params IEnumerable<object?> inputs)
        {
            foreach (object? outRef in inputs)
            {
                if (outRef != null)
                {
                    Type inputType = outRef.GetType();
                    if (inputType.Inherits<IOutRef>())
                    {
                        FieldInfo valueField = inputType.GetField(nameof(OutRef<object>.Var))!;
                        valueField.SetValue(
                            outRef,
                            _randomizer.Create(valueField.FieldType, _randomizer.Parent)
                        );
                    }
                }
            }
        }

        /// <summary>Randomizes any OutRef inputs and the return.</summary>
        /// <typeparam name="T">Return <see cref="Type"/> for the delegate.</typeparam>
        /// <param name="inputs">Inputs for the delegate.</param>
        /// <returns>Generated output for the delegate.</returns>
        private T? HandleInAndOut<T>(params IEnumerable<object?> inputs)
        {
            HandleIn(inputs);
            return (T?)_randomizer.Create(typeof(T), _randomizer.Parent);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate()
        {
            HandleIn();
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1>(T1? in1)
        {
            HandleIn(in1);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2>(T1? in1, T2? in2)
        {
            HandleIn(in1, in2);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3>(T1? in1, T2? in2, T3? in3)
        {
            HandleIn(in1, in2, in3);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4>(T1? in1, T2? in2, T3? in3, T4? in4)
        {
            HandleIn(in1, in2, in3, in4);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5>(T1? in1, T2? in2, T3? in3, T4? in4, T5? in5)
        {
            HandleIn(in1, in2, in3, in4, in5);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13,
            T14? in14
        )
        {
            HandleIn(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11, in12, in13, in14);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<
            T1,
            T2,
            T3,
            T4,
            T5,
            T6,
            T7,
            T8,
            T9,
            T10,
            T11,
            T12,
            T13,
            T14,
            T15
        >(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13,
            T14? in14,
            T15? in15
        )
        {
            HandleIn(
                in1,
                in2,
                in3,
                in4,
                in5,
                in6,
                in7,
                in8,
                in9,
                in10,
                in11,
                in12,
                in13,
                in14,
                in15
            );
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal void AutoDelegate<
            T1,
            T2,
            T3,
            T4,
            T5,
            T6,
            T7,
            T8,
            T9,
            T10,
            T11,
            T12,
            T13,
            T14,
            T15,
            T16
        >(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13,
            T14? in14,
            T15? in15,
            T16? in16
        )
        {
            HandleIn(
                in1,
                in2,
                in3,
                in4,
                in5,
                in6,
                in7,
                in8,
                in9,
                in10,
                in11,
                in12,
                in13,
                in14,
                in15,
                in16
            );
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<TOut>()
        {
            return HandleInAndOut<TOut>();
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, TOut>(T1 in1)
        {
            return HandleInAndOut<TOut>(in1);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, TOut>(T1? in1, T2? in2)
        {
            return HandleInAndOut<TOut>(in1, in2);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, TOut>(T1? in1, T2? in2, T3? in3)
        {
            return HandleInAndOut<TOut>(in1, in2, in3);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, TOut>(T1? in1, T2? in2, T3? in3, T4? in4)
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5
        )
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4, in5);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6
        )
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4, in5, in6);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, T7, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7
        )
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4, in5, in6, in7);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8
        )
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4, in5, in6, in7, in8);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9
        )
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4, in5, in6, in7, in8, in9);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10
        )
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11
        )
        {
            return HandleInAndOut<TOut>(in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11);
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12
        )
        {
            return HandleInAndOut<TOut>(
                in1,
                in2,
                in3,
                in4,
                in5,
                in6,
                in7,
                in8,
                in9,
                in10,
                in11,
                in12
            );
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13
        )
        {
            return HandleInAndOut<TOut>(
                in1,
                in2,
                in3,
                in4,
                in5,
                in6,
                in7,
                in8,
                in9,
                in10,
                in11,
                in12,
                in13
            );
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<
            T1,
            T2,
            T3,
            T4,
            T5,
            T6,
            T7,
            T8,
            T9,
            T10,
            T11,
            T12,
            T13,
            T14,
            TOut
        >(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13,
            T14? in14
        )
        {
            return HandleInAndOut<TOut>(
                in1,
                in2,
                in3,
                in4,
                in5,
                in6,
                in7,
                in8,
                in9,
                in10,
                in11,
                in12,
                in13,
                in14
            );
        }

        /// <inheritdoc cref="AutoDelegate{T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T}"/>
        internal TOut? AutoDelegate<
            T1,
            T2,
            T3,
            T4,
            T5,
            T6,
            T7,
            T8,
            T9,
            T10,
            T11,
            T12,
            T13,
            T14,
            T15,
            TOut
        >(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13,
            T14? in14,
            T15? in15
        )
        {
            return HandleInAndOut<TOut>(
                in1,
                in2,
                in3,
                in4,
                in5,
                in6,
                in7,
                in8,
                in9,
                in10,
                in11,
                in12,
                in13,
                in14,
                in15
            );
        }

        /// <summary>Matches to delegate with same number of inputs and output.</summary>
        /// <typeparam name="T1">Type for parameter 1.</typeparam>
        /// <typeparam name="T2">Type for parameter 2.</typeparam>
        /// <typeparam name="T3">Type for parameter 3.</typeparam>
        /// <typeparam name="T4">Type for parameter 4.</typeparam>
        /// <typeparam name="T5">Type for parameter 5.</typeparam>
        /// <typeparam name="T6">Type for parameter 6.</typeparam>
        /// <typeparam name="T7">Type for parameter 7.</typeparam>
        /// <typeparam name="T8">Type for parameter 8.</typeparam>
        /// <typeparam name="T9">Type for parameter 9.</typeparam>
        /// <typeparam name="T10">Type for parameter 10.</typeparam>
        /// <typeparam name="T11">Type for parameter 11.</typeparam>
        /// <typeparam name="T12">Type for parameter 12.</typeparam>
        /// <typeparam name="T13">Type for parameter 13.</typeparam>
        /// <typeparam name="T14">Type for parameter 14.</typeparam>
        /// <typeparam name="T15">Type for parameter 15.</typeparam>
        /// <typeparam name="T16">Type for parameter 16.</typeparam>
        /// <typeparam name="TOut">Type for the return value.</typeparam>
        /// <param name="in1">Matches input parameter 1.</param>
        /// <param name="in2">Matches input parameter 2.</param>
        /// <param name="in3">Matches input parameter 3.</param>
        /// <param name="in4">Matches input parameter 4.</param>
        /// <param name="in5">Matches input parameter 5.</param>
        /// <param name="in6">Matches input parameter 6.</param>
        /// <param name="in7">Matches input parameter 7.</param>
        /// <param name="in8">Matches input parameter 8.</param>
        /// <param name="in9">Matches input parameter 9.</param>
        /// <param name="in10">Matches input parameter 10.</param>
        /// <param name="in11">Matches input parameter 11.</param>
        /// <param name="in12">Matches input parameter 12.</param>
        /// <param name="in13">Matches input parameter 13.</param>
        /// <param name="in14">Matches input parameter 14.</param>
        /// <param name="in15">Matches input parameter 15.</param>
        /// <param name="in16">Matches input parameter 16.</param>
        /// <returns>Nothing if <see langword="void"/>; otherwise random data.</returns>
        internal TOut? AutoDelegate<
            T1,
            T2,
            T3,
            T4,
            T5,
            T6,
            T7,
            T8,
            T9,
            T10,
            T11,
            T12,
            T13,
            T14,
            T15,
            T16,
            TOut
        >(
            T1? in1,
            T2? in2,
            T3? in3,
            T4? in4,
            T5? in5,
            T6? in6,
            T7? in7,
            T8? in8,
            T9? in9,
            T10? in10,
            T11? in11,
            T12? in12,
            T13? in13,
            T14? in14,
            T15? in15,
            T16? in16
        )
        {
            return HandleInAndOut<TOut>(
                in1,
                in2,
                in3,
                in4,
                in5,
                in6,
                in7,
                in8,
                in9,
                in10,
                in11,
                in12,
                in13,
                in14,
                in15,
                in16
            );
        }
    }
}

#pragma warning restore S1144
