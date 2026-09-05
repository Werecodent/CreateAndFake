using System.Reflection;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.FakerTool.Proxy;

/// <summary>Creates dynamic subclasses.</summary>
internal static class Subclasser
{
    /// <summary>Prevents concurrency issues for <see cref="_TypeCache"/>.</summary>
    private static readonly Lock _Lock = new();

    /// <summary>Cache of already created types.</summary>
    private static readonly Dictionary<TypeInfo, Type[]> _TypeCache = [];

    /// <summary>Types not supported with the error message for them.</summary>
    private static readonly Dictionary<Type, string> _InvalidTypes = new()
    {
        { typeof(Array), $"Cannot subclass system reserved '{nameof(Array)}' type." },
        { typeof(ObjectDisposedException), $"{typeof(ObjectDisposedException)} not allowed." },
        { typeof(Task), $"{typeof(Task)} not supported without a provided constructor." },
#if LEGACY // Required feature shipped with C# 14 / .NET 10.0
        { typeof(TypeInfo), $"{typeof(TypeInfo)} itself has specific issues being faked." },
#endif
    };

    /// <summary>Assembly used to contain the dynamic types.</summary>
    public static AssemblyName AssemblyName => Emitter.AssemblyName;

    /// <summary>Determines if the type can be faked.</summary>
    /// <typeparam name="T">Type to check.</typeparam>
    /// <returns>True if possible; false otherwise.</returns>
    public static bool Supports<T>()
    {
        return Supports(typeof(T));
    }

    /// <summary>Determines if the type can be faked.</summary>
    /// <param name="type">Type to check.</param>
    /// <returns>True if possible; false otherwise.</returns>
    public static bool Supports(Type type)
    {
        return CanBeSubclassed(type).Item1;
    }

    /// <summary>Creates a subclass of the given type.</summary>
    /// <typeparam name="T">Parent type to inherit from.</typeparam>
    /// <param name="options">Configured options being used by the tool.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>An instance of the created child type.</returns>
    public static T Create<T>(FakerOptions options, params IEnumerable<Type> interfaces)
    {
        return (T)Create(typeof(T), options, interfaces);
    }

    /// <summary>Creates a subclass of the given type.</summary>
    /// <param name="parent">Parent type to inherit from.</param>
    /// <param name="options">Configured options being used by the tool.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>An instance of the created child type.</returns>
    public static IFaked Create(
        Type parent,
        FakerOptions options,
        params IEnumerable<Type> interfaces
    )
    {
        ArgumentGuard.ThrowIfNull(options);

        return (IFaked)
            CreateInfo(parent, interfaces)
                .AsType()
                .GetConstructor([Emitter.MetaType])!
                .Invoke([new FakeMetaProvider(options.Gen.Next<int>(), options)]);
    }

    /// <summary>Creates a subclass of the given type.</summary>
    /// <param name="parent">Parent type to inherit from.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>The created child type.</returns>
    /// <exception cref="ArgumentException"></exception>
    internal static TypeInfo CreateInfo(Type parent, params IEnumerable<Type> interfaces)
    {
        List<Type> allInterfaces = interfaces?.ToList() ?? [];
        allInterfaces.Add(Emitter.FakeType);

        Type realParent = parent ?? typeof(object);
        if (realParent.IsInterface)
        {
            allInterfaces.Add(realParent);
            realParent = typeof(object);
        }

        (bool, Exception?) possible = CanBeSubclassed(realParent);
        if (!possible.Item1)
        {
            throw possible.Item2!;
        }

        IEnumerable<Type> invalidInterfaces = allInterfaces.Where(t => !t.IsInterface);
        if (invalidInterfaces.Any())
        {
            throw new ArgumentException(
                "Given interfaces not actually interfaces: '"
                    + string.Join("', '", invalidInterfaces.Select(t => t.Name))
                    + "'.",
                nameof(interfaces)
            );
        }

        return FindOrBuildType(realParent, [.. allInterfaces.Distinct()]);
    }

    /// <summary>Determines if type can be subclassed.</summary>
    /// <param name="parent">Type to check.</param>
    /// <returns>True if possible; false if not with exception to throw.</returns>
    private static (bool, Exception?) CanBeSubclassed(Type parent)
    {
        TypeDescriber info = TypeDescriber.For(parent);

        if (parent == null)
        {
            return (true, null);
        }
        else if (_InvalidTypes.TryGetValue(parent, out string? error))
        {
            return (false, new ArgumentException(error, nameof(parent)));
        }
        else if (parent.IsSealed)
        {
            return (
                false,
                new ArgumentException(
                    $"Cannot subclass the sealed type '{parent.Name}'.",
                    nameof(parent)
                )
            );
        }
        else if (parent.IsPointer)
        {
            return (
                false,
                new ArgumentException(
                    $"Cannot subclass the pointer '{parent.Name}'.",
                    nameof(parent)
                )
            );
        }
        else if (parent.ContainsGenericParameters)
        {
            return (
                false,
                new ArgumentException(
                    $"Cannot subclass with unspecified generics '{parent.Name}'.",
                    nameof(parent)
                )
            );
        }
        else if (!ScopeChecker.IsVisible(parent, AssemblyName))
        {
            return (
                false,
                new ArgumentException(
                    $"Cannot subclass with nonpublic type '{parent.Name}'. Think about adding "
                        + $"'InternalsVisibleTo(\"{AssemblyName.Name}\")' to the type's assembly.",
                    nameof(parent)
                )
            );
        }
        else if (info.Inherits<Delegate>())
        {
            return (
                false,
                new ArgumentException($"Cannot subclass delegate '{parent}' type.", nameof(parent))
            );
        }
        else if (info.StaticMethods.All.Any(m => m.IsAbstract))
        {
            return (
                false,
                new ArgumentException("Static virtual members not yet supported.", nameof(parent))
            );
        }
        else if (parent.Namespace == "System.Numerics")
        {
            return (
                false,
                new ArgumentException(
                    "Numerics private static virtual members not supported.",
                    nameof(parent)
                )
            );
        }
        else
        {
            return (true, null);
        }
    }

    /// <summary>Finds or creates a type with the given hierarchy.</summary>
    /// <param name="parent">Parent type to inherit from.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <returns>The cached or created child type.</returns>
    private static TypeInfo FindOrBuildType(Type parent, Type[] interfaces)
    {
        lock (_Lock)
        {
            TypeInfo cachedType = _TypeCache
                .Where(t => t.Key.BaseType == parent)
                .SingleOrDefault(t =>
                    !t.Value.Except(interfaces).Union(interfaces.Except(t.Value)).Any()
                )
                .Key;

            if (cachedType != null)
            {
                return cachedType;
            }
            else
            {
                TypeInfo newType = Emitter.BuildType(parent, interfaces);
                _TypeCache.Add(newType, interfaces);
                return newType;
            }
        }
    }
}
