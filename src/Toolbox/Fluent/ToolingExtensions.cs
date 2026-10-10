using System.Numerics;
using Werecodent.CreateAndFake.Fluent.Tooling;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent randomization options.</summary>
public static class ToolingExtensions
{
    /// <summary>Add.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="tools"></param>
    /// <returns></returns>
    public static AsyncObjectTools<T> Tools<T>(this T instance, ToolSet? tools = null)
    {
        return new AsyncObjectTools<T>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<string> Tools(this string instance, ToolSet? tools = null)
    {
        return new ObjectTools<string>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static TypeTools Tools(this Type instance, ToolSet? tools = null)
    {
        return new TypeTools(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<float> Tools(this float instance, ToolSet? tools = null)
    {
        return new ObjectTools<float>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<double> Tools(this double instance, ToolSet? tools = null)
    {
        return new ObjectTools<double>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<decimal> Tools(this decimal instance, ToolSet? tools = null)
    {
        return new ObjectTools<decimal>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<TimeSpan> Tools(this TimeSpan instance, ToolSet? tools = null)
    {
        return new ObjectTools<TimeSpan>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<DateTime> Tools(this DateTime instance, ToolSet? tools = null)
    {
        return new ObjectTools<DateTime>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<BigInteger> Tools(this BigInteger instance, ToolSet? tools = null)
    {
        return new ObjectTools<BigInteger>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<DateTimeOffset> Tools(
        this DateTimeOffset instance,
        ToolSet? tools = null
    )
    {
        return new ObjectTools<DateTimeOffset>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<char> Tools(this char instance, ToolSet? tools = null)
    {
        return new ObjectTools<char>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<int> Tools(this int instance, ToolSet? tools = null)
    {
        return new ObjectTools<int>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<long> Tools(this long instance, ToolSet? tools = null)
    {
        return new ObjectTools<long>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<short> Tools(this short instance, ToolSet? tools = null)
    {
        return new ObjectTools<short>(instance, tools);
    }

    /// <inheritdoc cref="Tools{T}"/>
    public static ObjectTools<byte> Tools(this byte instance, ToolSet? tools = null)
    {
        return new ObjectTools<byte>(instance, tools);
    }
}
