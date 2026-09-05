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
    public static ObjectTools<T> Tools<T>(this T instance, ToolSet? tools = null)
    {
        return new ObjectTools<T>(instance, tools);
    }

    /// <summary>Add.</summary>
    /// <param name="type"></param>
    /// <param name="tools"></param>
    /// <returns></returns>
    public static TypeTools Tools(this Type type, ToolSet? tools = null)
    {
        return new TypeTools(type, tools);
    }
}
