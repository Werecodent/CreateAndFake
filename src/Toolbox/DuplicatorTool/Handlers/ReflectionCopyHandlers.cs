using System.Reflection;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

internal static class ReflectionCopyHandlers
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
    [
        new RefCopyHandler(typeof(Type)),
        new RefCopyHandler(typeof(ConstructorInfo)),
        new RefCopyHandler(typeof(MethodInfo)),
        new RefCopyHandler(typeof(PropertyInfo)),
        new RefCopyHandler(typeof(FieldInfo)),
        new RefCopyHandler(typeof(ParameterInfo)),
        new RefCopyHandler(typeof(Assembly)),
        new RefCopyHandler(typeof(AssemblyName)),
        new RefCopyHandler( // RuntimeType
            typeof(Type).GetType()
        ),
        new RefCopyHandler( // RuntimeConstructorInfo
            typeof(string).GetConstructors()[0].GetType()
        ),
        new RefCopyHandler( // RuntimeMethodInfo
            typeof(string).GetMethods()[0].GetType()
        ),
        new RefCopyHandler( // RuntimePropertyInfo
            typeof(string).GetProperties()[0].GetType()
        ),
        new RefCopyHandler( // RtFieldInfo
            typeof(string).GetFields()[0].GetType()
        ),
        new RefCopyHandler( // MdFieldInfo
            typeof(int).GetFields()[0].GetType()
        ),
        new RefCopyHandler( // RuntimeParameterInfo
            typeof(string).GetMethods().SelectMany(m => m.GetParameters()).First().GetType()
        ),
        new RefCopyHandler( // RuntimeAssembly
            AppDomain.CurrentDomain.GetAssemblies()[0].GetType()
        ),
    ];
}
