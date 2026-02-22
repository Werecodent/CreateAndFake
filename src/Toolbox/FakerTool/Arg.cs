using CreateAndFake.Design.Comparisons;

namespace CreateAndFake.FakerTool;

/// <summary>Provides the ability to match arguments based upon conditions.</summary>
public sealed class Arg : IDeepCloneable
{
    /// <summary>Current setup args.</summary>
    private static readonly ThreadLocal<IList<Tuple<Arg, object?>>> _ArgCache = new(() => []);

    /// <summary>Condition to compare with.</summary>
    private readonly Func<object?, bool> _matcher;

    /// <inheritdoc cref="Arg"/>
    /// <param name="matcher">Condition to compare with.</param>
    private Arg(Func<object?, bool> matcher)
    {
        _matcher = matcher;
    }

    /// <inheritdoc/>
    public IDeepCloneable DeepClone()
    {
        return new Arg(_matcher);
    }

    /// <summary>Determines if the condition is matched.</summary>
    /// <param name="arg">Argument to compare with.</param>
    /// <returns>True if matched; false otherwise.</returns>
    internal bool Matches(object? arg)
    {
        return _matcher(arg);
    }

    /// <summary>Grabs created args and restarts arg tracking.</summary>
    /// <returns>Previously created args and associated values.</returns>
    internal static Tuple<Arg, object>[] CaptureSetArgs()
    {
        Tuple<Arg, object>[] result = [.. _ArgCache.Value!];
        _ArgCache.Value!.Clear();
        return result;
    }

    /// <summary>Matches any <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <returns>Random instance of <typeparamref name="T"/> for the fake setup.</returns>
    /// <remarks>Use <see cref="LambdaAny{T}"/> when matching via method names.</remarks>
    public static T? Any<T>()
    {
        T? value = Tools.Randomizer.Create<T>();
        _ArgCache.Value!.Add(Tuple.Create(LambdaAny<T>(), (object?)value));
        return value;
    }

    /// <summary>Matches any <typeparamref name="T"/> reference.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <returns>Container for the reference.</returns>
    /// <remarks>Use <see cref="LambdaAnyRef{T}"/> when matching via method names.</remarks>
    public static OutRef<T> AnyRef<T>()
    {
        return new OutRef<T>();
    }

    /// <summary>Matches any <typeparamref name="T"/> instance except <see langword="null"/>.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <returns>Random instance of <typeparamref name="T"/> for the fake setup.</returns>
    /// <remarks>Use <see cref="LambdaNotNull{T}"/> when matching via method names.</remarks>
    public static T? NotNull<T>()
        where T : class
    {
        T? value = Tools.Randomizer.Create<T>();
        _ArgCache.Value!.Add(Tuple.Create(LambdaNotNull<T>(), (object?)value));
        return value;
    }

    /// <summary>Matches any <typeparamref name="T"/> instance that fulfills <paramref name="condition"/>.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <param name="condition">Condition to verify.</param>
    /// <returns>Random instance of <typeparamref name="T"/> for the fake setup.</returns>
    /// <remarks>Use <see cref="LambdaWhere{T}"/> when matching via method names.</remarks>
    public static T? Where<T>(Func<T, bool> condition)
    {
        T? value = Tools.Randomizer.Create<T>();
        _ArgCache.Value!.Add(Tuple.Create(LambdaWhere(condition), (object?)value));
        return value;
    }

#pragma warning disable IDE0060, RCS1163 // Used via lambdas for setup replacement behavior.

    /// <summary>
    ///     Matches any <typeparamref name="T"/> reference that fulfills <paramref name="condition"/>.
    /// </summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <param name="condition">Condition to verify.</param>
    /// <returns>Container for the reference.</returns>
    /// <remarks>Use <see cref="LambdaWhereRef{T}"/> when matching via method names.</remarks>
    public static OutRef<T> WhereRef<T>(Func<T, bool> condition)
    {
        return new OutRef<T>();
    }

#pragma warning restore IDE0060, RCS1163

    /// <summary>Matches any <typeparamref name="T"/> instance.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <returns>Arg for the fake provider to match with.</returns>
    public static Arg LambdaAny<T>()
    {
        return new Arg(o => o is T or null);
    }

    /// <summary>Matches any <typeparamref name="T"/> reference.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <returns>Arg for the fake provider to match with.</returns>
    public static Arg LambdaAnyRef<T>()
    {
        return LambdaWhere<OutRef<T>>(null);
    }

    /// <summary>Matches any <typeparamref name="T"/> instance except <see langword="null"/>.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <returns>Arg for the fake provider to match with.</returns>
    public static Arg LambdaNotNull<T>()
        where T : class
    {
        return LambdaWhere<T>(o => o != null);
    }

    /// <summary>Matches any <typeparamref name="T"/> instance that fulfills <paramref name="condition"/>.</summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <param name="condition">Condition to verify.</param>
    /// <returns>Arg for the fake provider to match with.</returns>
    public static Arg LambdaWhere<T>(Func<T, bool>? condition)
    {
        return new Arg(o => o is T value && (condition?.Invoke(value) ?? true));
    }

    /// <summary>
    ///     Matches any <typeparamref name="T"/> reference that fulfills <paramref name="condition"/>.
    /// </summary>
    /// <typeparam name="T">Type to match.</typeparam>
    /// <param name="condition">Condition to verify.</param>
    /// <returns>Container for the reference.</returns>
    public static Arg LambdaWhereRef<T>(Func<T, bool>? condition)
    {
        return LambdaWhere<OutRef<T>>(o => condition?.Invoke(o.Var!) ?? true);
    }
}
