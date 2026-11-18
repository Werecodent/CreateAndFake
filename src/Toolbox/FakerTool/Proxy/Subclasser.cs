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
        { typeof(Uri), $"{typeof(Uri)} not supported without a provided constructor." },
        { typeof(Task), $"{typeof(Task)} not supported without a provided constructor." },
        { typeof(ValueTask), $"{typeof(ValueTask)} not supported without a provided constructor." },
        {
            typeof(ValueTask?),
            $"{typeof(ValueTask?)} not supported without a provided constructor."
        },
        { typeof(Task<>), $"{typeof(Task<>)} not supported without a provided constructor." },
        {
            typeof(ValueTask<>),
            $"{typeof(ValueTask<>)} not supported without a provided constructor."
        },
        {
            typeof(StreamReader),
            $"{typeof(StreamReader)} not supported without a provided constructor."
        },
        {
            typeof(StreamWriter),
            $"{typeof(StreamWriter)} not supported without a provided constructor."
        },
#if NET11_0_OR_GREATER
        { typeof(FieldInfo), $"{typeof(FieldInfo)} itself has specific issues being faked." },
#endif
#if LEGACY // Required feature shipped with C# 14 / .NET 10.0
        { typeof(TypeInfo), $"{typeof(TypeInfo)} itself has specific issues being faked." },
#endif
    };

    /// <summary>Type states not supported with the error message for them.</summary>
    private static readonly Dictionary<Func<Type, bool>, Func<Type, string>> _InvalidTypeStates =
        new()
        {
            { p => p.IsSealed, p => $"Cannot subclass the sealed type '{p.Name}'." },
            { p => p.IsPointer, p => $"Cannot subclass the pointer '{p.Name}'." },
            {
                p => p.ContainsGenericParameters,
                p => $"Cannot subclass with unspecified generics '{p.Name}'."
            },
            {
                p => !ScopeChecker.IsVisible(p, AssemblyName),
                p =>
                    $"Cannot subclass with nonpublic type '{p.Name}'. Think about adding "
                    + $"'InternalsVisibleTo(\"{AssemblyName.Name}\")' to the type's assembly."
            },
            {
                p => p.Namespace == "System.Numerics",
                p => $"Numerics private static virtual members not supported for type '{p.Name}'."
            },
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
        return type != null && CanBeSubclassed(type).Item1;
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

        if (
            _InvalidTypes.TryGetValue(
                GenericConverter.AsGenericBase(parent) ?? parent,
                out string? error
            )
        )
        {
            return (false, new ArgumentException(error, nameof(parent)));
        }

        Func<Type, string>? invalidStateMessage = _InvalidTypeStates
            .FirstOrDefault(s => s.Key(parent))
            .Value;
        if (invalidStateMessage != null)
        {
            return (false, new ArgumentException(invalidStateMessage(parent), nameof(parent)));
        }

        if (info.Inherits<Delegate>())
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
        else if (
            info.Properties.Settable.Any(p =>
                p.SetMethod!.ReturnParameter.GetRequiredCustomModifiers()
                    .Contains(typeof(System.Runtime.CompilerServices.IsExternalInit))
            )
        )
        {
            return (
                false,
                new ArgumentException(
                    "Classes with init properties not yet supported.",
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
