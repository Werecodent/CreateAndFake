using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Design.Content;

/// <summary>Finds details for types.</summary>
public static class TypeDescriber
{
    /// <summary>Caches every available <see cref="Type"/> per <see cref="Assembly"/>.</summary>
    private static readonly Dictionary<Assembly, ImmutableArray<Type>> _ClassTypeCache = [];

    /// <typeparam name="T"><see cref="Type"/> to find the generic base of.</typeparam>
    /// <inheritdoc cref="FindConcreteInterface(Type,Type)"/>
    public static Type FindConcreteInterface<T>(Type genericBase)
    {
        return FindConcreteInterface(typeof(T), genericBase);
    }

    /// <summary>
    ///     Finds the defined <see langword="interface"/> with generics
    ///     specified on the type (<see langword="this"/>).
    /// </summary>
    /// <param name="type"><see cref="Type"/> to find the generic base of.</param>
    /// <param name="genericBase">
    ///     Generic <see cref="Type"/> definition without specified generics.
    /// </param>
    /// <returns>The found defined <see langword="interface"/>.</returns>
    /// <exception cref="InvalidOperationException">If null or missing.</exception>
    public static Type FindConcreteInterface(Type? type, Type genericBase)
    {
        return type?.GetInterfaces()
                .Where(i => i.IsGenericType)
                .SingleOrDefault(i => i.GetGenericTypeDefinition() == genericBase)
            ?? throw new InvalidOperationException($"Type {type} doesn't inherit {genericBase}.");
    }

    /// <inheritdoc cref="GetAllFields{T}(BindingFlags)"/>
    public static IEnumerable<FieldInfo> GetAllFields<T>()
    {
        return GetAllFields(typeof(T));
    }

    /// <typeparam name="T"><see cref="Type"/> to find fields on.</typeparam>
    /// <inheritdoc cref="GetAllFields(Type,BindingFlags)"/>
    public static IEnumerable<FieldInfo> GetAllFields<T>(BindingFlags scope)
    {
        return GetAllFields(typeof(T), scope);
    }

    /// <inheritdoc cref="GetAllFields(Type,BindingFlags)"/>
    public static IEnumerable<FieldInfo> GetAllFields(Type? type)
    {
        return GetAllFields(type, BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Get all fields.</summary>
    /// <param name="type"><see cref="Type"/> to find fields on.</param>
    /// <param name="scope">Member scope to filter results on.</param>
    /// <returns>All fields.</returns>
    public static IEnumerable<FieldInfo> GetAllFields(Type? type, BindingFlags scope)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | scope))
        {
            yield return field;
        }

        if (scope.HasFlag(BindingFlags.NonPublic))
        {
            Type? baseType = type.BaseType;
            int i = 0;
            while (baseType != null && baseType != typeof(object) && i++ < 10)
            {
                foreach (
                    FieldInfo field in baseType.GetFields(
                        BindingFlags.Instance | BindingFlags.NonPublic
                    )
                )
                {
                    if (field.IsPrivate)
                    {
                        yield return field;
                    }
                }
                baseType = baseType.BaseType;
            }
        }
    }

    /// <inheritdoc cref="GetAllProperties{T}(BindingFlags)"/>
    public static IEnumerable<PropertyInfo> GetAllProperties<T>()
    {
        return GetAllProperties(typeof(T));
    }

    /// <typeparam name="T"><see cref="Type"/> to find properties on.</typeparam>
    /// <inheritdoc cref="GetAllProperties(Type,BindingFlags)"/>
    public static IEnumerable<PropertyInfo> GetAllProperties<T>(BindingFlags scope)
    {
        return GetAllProperties(typeof(T), scope);
    }

    /// <inheritdoc cref="GetAllProperties(Type,BindingFlags)"/>
    public static IEnumerable<PropertyInfo> GetAllProperties(Type? type)
    {
        return GetAllProperties(type, BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Get all properties.</summary>
    /// <param name="type"><see cref="Type"/> to find properties on.</param>
    /// <param name="scope">Member scope to filter results on.</param>
    /// <returns>All properties.</returns>
    public static IEnumerable<PropertyInfo> GetAllProperties(Type? type, BindingFlags scope)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Instance | scope))
        {
            yield return prop;
        }

        if (scope.HasFlag(BindingFlags.NonPublic))
        {
            Type? baseType = type.BaseType;
            int i = 0;
            while (baseType != null && baseType != typeof(object) && i++ < 10)
            {
                foreach (
                    PropertyInfo prop in baseType.GetProperties(
                        BindingFlags.Instance | BindingFlags.NonPublic
                    )
                )
                {
                    if (prop.CanRead && (prop.GetGetMethod()?.IsPrivate ?? false))
                    {
                        yield return prop;
                    }
                }
                baseType = baseType.BaseType;
            }
        }
    }

    /// <typeparam name="T"><see cref="Type"/> to check.</typeparam>
    /// <inheritdoc cref="FindLocalSubclasses(Type)"/>
    public static IEnumerable<Type> FindLocalSubclasses<T>()
    {
        return FindLocalSubclasses(typeof(T));
    }

    /// <summary>
    ///     Finds subclasses of the type (<see langword="this"/>)
    ///     from its defined <see cref="Assembly"/>.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <returns>The found creatable subclasses for the type.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public static IEnumerable<Type> FindLocalSubclasses(Type? type)
    {
        if (type == null)
        {
            return [];
        }

        return FindLoadedClassTypes(type.Assembly)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(type))
            .Where(t => IsVisible(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <typeparam name="T"><see cref="Type"/> to check.</typeparam>
    /// <inheritdoc cref="FindLoadedSubclasses(Type)"/>
    public static IEnumerable<Type> FindLoadedSubclasses<T>()
    {
        return FindLoadedSubclasses(typeof(T));
    }

    /// <summary>
    ///     Finds subclasses of the type (<see langword="this"/>)
    ///     in every loaded <see cref="Assembly"/>.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <returns>The found creatable subclasses for the type.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public static IEnumerable<Type> FindLoadedSubclasses(Type? type)
    {
        return AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => !a.ReflectionOnly)
            .Where(a => !a.IsDynamic)
            .SelectMany(FindLoadedClassTypes)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(type))
            .Where(t => IsVisible(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <summary>
    ///     Find every <see langword="class"/> <see cref="Type"/> in <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly"><see cref="Type"/> container to search.</param>
    /// <returns>The found types if <paramref name="assembly"/> can load, none otherwise.</returns>
    internal static IEnumerable<Type> FindLoadedClassTypes(Assembly? assembly)
    {
        if (assembly == null)
        {
            return [];
        }

        ImmutableArray<Type> classTypes;
        lock (_ClassTypeCache)
        {
            if (!_ClassTypeCache.TryGetValue(assembly, out classTypes))
            {
                IEnumerable<Type> types = FindLoadedTypes(assembly)
                    .Where(t => t.IsClass)
                    .Where(t => !t.IsNestedPrivate)
                    .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), false));

                _ClassTypeCache[assembly] = classTypes = [.. types];
            }
        }
        return classTypes;
    }

    /// <summary>Finds every <see cref="Type"/> in <paramref name="assembly"/>.</summary>
    /// <param name="assembly"><see cref="Type"/> container to search.</param>
    /// <returns>The found types if <paramref name="assembly"/> can load, none otherwise.</returns>
    internal static IEnumerable<Type> FindLoadedTypes(Assembly? assembly)
    {
        try
        {
            return assembly?.GetTypes() ?? Type.EmptyTypes;
        }
        catch (FileNotFoundException)
        {
            return Type.EmptyTypes;
        }
        catch (ReflectionTypeLoadException)
        {
            return Type.EmptyTypes;
        }
    }

    /// <typeparam name="T"><see cref="Type"/> to check.</typeparam>
    /// <inheritdoc cref="IsVisible(Type,AssemblyName)"/>
    public static bool IsVisible<T>(AssemblyName assembly)
    {
        return IsVisible(typeof(T), assembly);
    }

    /// <summary>
    ///     Determines if <paramref name="type"/> (<see langword="this"/>)
    ///     can be used by <paramref name="assembly"/>.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <param name="assembly">Name of the <see cref="Assembly"/> to verify access for.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="type"/> is visible to
    ///     <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark the <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c> to
    ///     return <see langword="true"/> for its <see langword="internal"/> types with this method.
    /// </remarks>
    public static bool IsVisible(Type? type, AssemblyName assembly)
    {
        return type != null
            && (
                type.IsVisible
                || type.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
                    .Any(a => a.AssemblyName == assembly.Name)
            );
    }
}
