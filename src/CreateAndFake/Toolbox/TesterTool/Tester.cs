using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.FakerTool;

namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <c>null</c> parameter.</exception>
public class Tester(TesterOptions options)
{
    /// <inheritdoc/>
    public TesterOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

    /// <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each nullable parameter possible with null.
    ///     Constructor and factory parameters are additionally tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="injectionValues">Values to inject into the method.</param>
    public virtual void PreventsNullRefException<T>(params object[] injectionValues)
    {
        PreventsNullRefException(typeof(T), injectionValues);
    }

    /// <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each nullable parameter possible with null.
    ///     Constructor and factory parameters are additionally tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    public virtual void PreventsNullRefException(Type type, params object[] injectionValues)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        NullGuarder checker = new(new GenericFixer(Options.Gen, Options.Randomizer), Options.Randomizer, Options.Asserter, Options.Timeout);

        Options.Limiter.Retry<TimeoutException>($"Null reference check on constructors for type '{type}'",
            () => checker.PreventsNullRefExceptionOnConstructors(type, true, injectionValues)).Wait();

        if (!(type.IsAbstract && type.IsSealed))
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Null reference check on methods for type '{type}'",
                () =>
                {
                    object? instance = (injectionValues?.Length > 0)
                        ? Options.Randomizer.Inject(type, injectionValues)
                        : Options.Randomizer.Create(type);
                    try
                    {
                        checker.PreventsNullRefExceptionOnMethods(instance!, injectionValues);
                    }
                    finally
                    {
                        Disposer.Cleanup(instance);
                    }
                }).Wait();
        }

        Options.Limiter.Retry<TimeoutException>($"Null reference check on static methods for type '{type}'",
            () => checker.PreventsNullRefExceptionOnStatics(type, true, injectionValues)).Wait();
    }

    /// <summary>
    ///     Verifies nulls are guarded on the type.
    ///     Tests each parameter possible with null.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception besides NullReferenceException and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    public virtual void PreventsNullRefException<T>(T instance, params object[] injectionValues)
    {
        NullGuarder checker = new(new GenericFixer(Options.Gen, Options.Randomizer), Options.Randomizer, Options.Asserter, Options.Timeout);

        Options.Limiter.Retry<TimeoutException>($"Null reference check on constructors for type '{typeof(T).Name}'",
            () => checker.PreventsNullRefExceptionOnConstructors(typeof(T), false, injectionValues)).Wait();
        Options.Limiter.Retry<TimeoutException>($"Null reference check on methods for type '{typeof(T).Name}'",
            () => checker.PreventsNullRefExceptionOnMethods(instance!, injectionValues)).Wait();
        Options.Limiter.Retry<TimeoutException>($"Null reference check on static methods for type '{typeof(T).Name}'",
            () => checker.PreventsNullRefExceptionOnStatics(typeof(T), false, injectionValues)).Wait();
    }

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are additionally tested by running all methods.
    ///     Ignores any exception and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="injectionValues">Values to inject into the method.</param>
    public virtual void PreventsParameterMutation<T>(params object[] injectionValues)
    {
        PreventsParameterMutation(typeof(T), injectionValues);
    }

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are additionally tested by running all methods.
    ///     Ignores any exception and moves on.
    /// </summary>
    /// <param name="type">Type to verify.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    public virtual void PreventsParameterMutation(Type type, params object[] injectionValues)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        MutationGuarder checker = new(new GenericFixer(Options.Gen, Options.Randomizer),
            Options.Randomizer, Options.Duplicator, Options.Asserter, Options.Timeout);

        Options.Limiter.Retry<TimeoutException>($"Parameter mutation check on constructors for type '{type}'",
            () => checker.PreventsMutationOnConstructors(type, true, injectionValues)).Wait();

        if (!(type.IsAbstract && type.IsSealed))
        {
            Options.Limiter.Retry<TimeoutException>(
                $"Parameter mutation check on methods for type '{type}'",
                () =>
                {
                    object? instance = (injectionValues?.Length > 0)
                        ? Options.Randomizer.Inject(type, injectionValues)
                        : Options.Randomizer.Create(type);
                    try
                    {
                        checker.PreventsMutationOnMethods(instance!, injectionValues);
                    }
                    finally
                    {
                        Disposer.Cleanup(instance);
                    }
                }).Wait();
        }

        Options.Limiter.Retry<TimeoutException>($"Parameter mutation check on static methods for type '{type}'",
            () => checker.PreventsMutationOnStatics(type, true, injectionValues)).Wait();
    }

    /// <summary>
    ///     Verifies mutations are prevented on the type.
    ///     Constructor and factory parameters are not tested by running all methods.
    ///     Ignores any exception and moves on.
    /// </summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="injectionValues">Values to inject into the method.</param>
    public virtual void PreventsParameterMutation<T>(T instance, params object[] injectionValues)
    {
        MutationGuarder checker = new(new GenericFixer(Options.Gen, Options.Randomizer),
            Options.Randomizer, Options.Duplicator, Options.Asserter, Options.Timeout);

        Options.Limiter.Retry<TimeoutException>($"Parameter mutation check on constructors for type '{typeof(T).Name}'",
            () => checker.PreventsMutationOnConstructors(typeof(T), false, injectionValues)).Wait();
        Options.Limiter.Retry<TimeoutException>($"Parameter mutation check on methods for type '{typeof(T).Name}'",
            () => checker.PreventsMutationOnMethods(instance!, injectionValues)).Wait();
        Options.Limiter.Retry<TimeoutException>($"Parameter mutation check on static methods for type '{typeof(T).Name}'",
            () => checker.PreventsMutationOnStatics(typeof(T), false, injectionValues)).Wait();
    }

    /// <summary>Verifies no exceptions are thrown on any method when using injection and random data.</summary>
    /// <typeparam name="T">Type to verify.</typeparam>
    /// <param name="injectionValues">Values to inject into called methods.</param>
    public virtual void PassthroughWithNoExceptions<T>(params object[] injectionValues)
    {
        PassthroughWithNoExceptions(Options.Randomizer.Create<Injected<T>>()!.Dummy!, injectionValues);
    }

    /// <summary>Verifies no exceptions are thrown on any method.</summary>
    /// <param name="instance">Instance to test the methods on.</param>
    /// <param name="injectionValues">Values to inject into called methods.</param>
    public virtual void PassthroughWithNoExceptions(object instance, params object[] injectionValues)
    {
        new ExceptionGuarder(new GenericFixer(Options.Gen, Options.Randomizer), Options.Randomizer, Options.Asserter, Options.Timeout)
            .CallAllMethods(instance, injectionValues);
    }
}
