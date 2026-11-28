using System.Numerics;
using Werecodent.CreateAndFake.Fluent.Tooling;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent tool actions.</summary>
public static class ToolingExtensions
{
    /// <summary>Accesses available tool behavior for the instance.</summary>
    /// <typeparam name="T">Type being used.</typeparam>
    /// <param name="instance">Instance to act upon.</param>
    /// <param name="tools">Custom tools to potentially use.</param>
    /// <returns>Tool functions to run.</returns>
    public static AsyncObjectTools<T> Tools<T>(this T instance, IToolSet? tools = null)
    {
        return new AsyncObjectTools<T>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<string> Tools(this string instance, IToolSet? tools = null)
    {
        return new ObjectTools<string>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static TypeTools Tools(this Type instance, IToolSet? tools = null)
    {
        return new TypeTools(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<float> Tools(this float instance, IToolSet? tools = null)
    {
        return new ObjectTools<float>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<double> Tools(this double instance, IToolSet? tools = null)
    {
        return new ObjectTools<double>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<decimal> Tools(this decimal instance, IToolSet? tools = null)
    {
        return new ObjectTools<decimal>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<TimeSpan> Tools(this TimeSpan instance, IToolSet? tools = null)
    {
        return new ObjectTools<TimeSpan>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<DateTime> Tools(this DateTime instance, IToolSet? tools = null)
    {
        return new ObjectTools<DateTime>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<BigInteger> Tools(this BigInteger instance, IToolSet? tools = null)
    {
        return new ObjectTools<BigInteger>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<DateTimeOffset> Tools(
        this DateTimeOffset instance,
        IToolSet? tools = null
    )
    {
        return new ObjectTools<DateTimeOffset>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<char> Tools(this char instance, IToolSet? tools = null)
    {
        return new ObjectTools<char>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<int> Tools(this int instance, IToolSet? tools = null)
    {
        return new ObjectTools<int>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<long> Tools(this long instance, IToolSet? tools = null)
    {
        return new ObjectTools<long>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<short> Tools(this short instance, IToolSet? tools = null)
    {
        return new ObjectTools<short>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<byte> Tools(this byte instance, IToolSet? tools = null)
    {
        return new ObjectTools<byte>(instance, tools);
    }
}
