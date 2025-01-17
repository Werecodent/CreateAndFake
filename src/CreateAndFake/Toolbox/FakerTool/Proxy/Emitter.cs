using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Toolbox.FakerTool.Proxy;

/// <summary>Creates dynamic subclasses by IL code.</summary>
internal static class Emitter
{
    /// <summary>Assembly used to contain the dynamic types.</summary>
    internal static AssemblyName AssemblyName { get; } = new AssemblyName("FakerTypes");

    /// <summary>Underlying type managing implementation details.</summary>
    internal static Type FakeType { get; } = typeof(IFaked);

    /// <summary>Underlying type managing implementation details.</summary>
    internal static Type MetaType { get; } = typeof(FakeMetaProvider);

    /// <summary>Flags used to find members to implement.</summary>
    private const BindingFlags _MemberFinder = BindingFlags.FlattenHierarchy |
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    /// <summary>Methods called to chain fake calls.</summary>
    private static readonly MethodInfo
        _VoidChainer = MetaType.GetMethod(
            nameof(FakeMetaProvider.CallVoid),
            BindingFlags.Instance | BindingFlags.NonPublic)!,
        _ResultChainer = MetaType.GetMethod(
            nameof(FakeMetaProvider.CallRet),
            BindingFlags.Instance | BindingFlags.NonPublic)!,
        _TypeResolver = typeof(Type).GetMethod(
            nameof(Type.GetTypeFromHandle),
            BindingFlags.Static | BindingFlags.Public)!;

    /// <summary>Module storing the faked types.</summary>
    private static readonly ModuleBuilder _Module = AssemblyBuilder
        .DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.RunAndCollect)
        .DefineDynamicModule("FakerTypesModule");

    /// <summary>Creates a type with the given inheritance.</summary>
    /// <param name="parent">Base class inheriting from.</param>
    /// <param name="interfaces">Additional interfaces to inherit.</param>
    /// <returns>Dynamic type with behavior faked.</returns>
    internal static TypeInfo BuildType(Type parent, Type[] interfaces)
    {
        TypeBuilder newType = _Module.DefineType(
            "Fake_" + string.Join("|", interfaces.Prepend(parent).Select(i => i.Name)) + "_" + Guid.NewGuid(),
            TypeAttributes.NotPublic | TypeAttributes.Sealed, parent, interfaces);

        MethodInfo metaGetter = SetupFakeMetaProvider(newType, SetupConstructor(newType, parent));

        (PropertyInfo, PropertyBuilder)[] props = [.. FindImplementableProperties(interfaces.Prepend(parent))
            .Select(p => (p, newType.DefineProperty(
                p.DeclaringType!.Name + "." + p.Name,
                p.Attributes,
                p.PropertyType,
                Type.EmptyTypes)))];

        foreach (MethodInfo method in FindImplementableMethods(interfaces.Prepend(parent)))
        {
            MethodBuilder fakedMethod = newType.DefineMethod(
                    method.DeclaringType!.Name + "." + method.Name,
                    method.Attributes & ~MethodAttributes.Abstract,
                    method.ReturnType,
                    [.. method.GetParameters().Select(p => p.ParameterType)]);

            if (method.IsGenericMethod)
            {
                _ = fakedMethod.DefineGenericParameters([.. method.GetGenericArguments().Select(a => a.Name)]);
            }
            ImplementFakeBehavior(fakedMethod.GetILGenerator(), method, metaGetter);
            newType.DefineMethodOverride(fakedMethod, method);

            foreach ((PropertyInfo, PropertyBuilder) prop in props)
            {
                if (prop.Item1.GetMethod == method)
                {
                    prop.Item2.SetGetMethod(fakedMethod);
                }
                else if (prop.Item1.SetMethod == method)
                {
                    prop.Item2.SetSetMethod(fakedMethod);
                }
            }
        }

        return newType.CreateTypeInfo()!;
    }

    /// <summary>Finds all properties to generate for a type implementing the given interfaces.</summary>
    /// <param name="interfaces">Interfaces to inherit.</param>
    /// <returns>Properties to implement for the faked type.</returns>
    private static IEnumerable<PropertyInfo> FindImplementableProperties(IEnumerable<Type> interfaces)
    {
        return interfaces
            .Where(t => t != typeof(IFaked))
            .SelectMany(i => i
                .GetProperties(_MemberFinder)
                .Where(p => IsVisible(p.GetMethod))
                .Concat(FindImplementableProperties(i.GetInterfaces())))
            .Distinct();
    }

    /// <summary>Finds all methods to generate for a type implementing the given interfaces.</summary>
    /// <param name="interfaces">Interfaces to inherit.</param>
    /// <returns>Methods to implement for the faked type.</returns>
    private static IEnumerable<MethodInfo> FindImplementableMethods(IEnumerable<Type> interfaces)
    {
        return interfaces
            .Where(t => t != typeof(IFaked))
            .SelectMany(i => i
                .GetMethods(_MemberFinder)
                .Where(m => m.IsAbstract || (m.IsVirtual && !m.IsFinal))
                .Where(m => IsVisible(m))
                .Where(m => m.Name != "Finalize")
                .Concat(FindImplementableMethods(i.GetInterfaces())))
            .Distinct();
    }

    /// <summary>Determines if the method is visible for faking.</summary>
    /// <param name="method">Method to check.</param>
    /// <returns>True if visible; false otherwise.</returns>
    private static bool IsVisible(MethodInfo? method)
    {
        if (method == null)
        {
            return true;
        }

        return !method.IsPrivate
            && (!(method.IsAssembly || method.IsFamilyAndAssembly)
                || method.Module.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
                    .Any(a => a.AssemblyName == AssemblyName.Name));
    }

    /// <summary>Chains the call for a method.</summary>
    /// <param name="gen">Generator for the fake method.</param>
    /// <param name="method">Method being implemented.</param>
    /// <param name="metaGetter">Hook for the meta provider to chain with.</param>
    private static void ImplementFakeBehavior(ILGenerator gen, MethodInfo method, MethodInfo metaGetter)
    {
        ParameterInfo[] argInfos = method.GetParameters();
        Type[] generics = method.GetGenericArguments();

        // object[] args = new object[params.Length];
        LocalBuilder args = gen.DeclareLocal(typeof(object[]));
        gen.Emit(OpCodes.Ldc_I4, argInfos.Length);
        gen.Emit(OpCodes.Newarr, typeof(object));
        gen.Emit(OpCodes.Stloc, args);

        // args[0..x] = params[0..x];
        for (int i = 0; i < argInfos.Length; i++)
        {
            gen.Emit(OpCodes.Ldloc, args);
            gen.Emit(OpCodes.Ldc_I4, i);
            if (!argInfos[i].ParameterType.IsByRef)
            {
                gen.Emit(OpCodes.Ldarg, i + 1);
                gen.Emit(OpCodes.Box, argInfos[i].ParameterType);
                gen.Emit(OpCodes.Stelem_Ref);
            }
            else
            {
                // args[i] = new OutRef<T>();
                Type outRef = typeof(OutRef<>).MakeGenericType(argInfos[i].ParameterType.GetElementType()!);
                gen.Emit(OpCodes.Newobj, outRef.GetConstructor(Type.EmptyTypes)!);
                gen.Emit(OpCodes.Stelem_Ref);
                if (!argInfos[i].IsOut)
                {
                    // ((OutRef<T>)args[i].Var) = params[i];
                    gen.Emit(OpCodes.Ldloc, args);
                    gen.Emit(OpCodes.Ldc_I4, i);
                    gen.Emit(OpCodes.Ldelem_Ref);
                    gen.Emit(OpCodes.Castclass, outRef);
                    gen.Emit(OpCodes.Ldarg, i + 1);
                    gen.Emit(OpCodes.Ldind_Ref);
                    gen.Emit(OpCodes.Stfld, outRef.GetField(nameof(OutRef<Type>.Var))!);
                }
            }
        }

        // Type[] types = new Type[generics.Length];
        LocalBuilder types = gen.DeclareLocal(typeof(Type[]));
        gen.Emit(OpCodes.Ldc_I4, generics.Length);
        gen.Emit(OpCodes.Newarr, typeof(Type));
        gen.Emit(OpCodes.Stloc, types);

        // types[0..x] = typeof(generics[0..x]);
        for (int i = 0; i < generics.Length; i++)
        {
            gen.Emit(OpCodes.Ldloc, types);
            gen.Emit(OpCodes.Ldc_I4, i);
            gen.Emit(OpCodes.Ldtoken, generics[i]);
            gen.Emit(OpCodes.Call, _TypeResolver);
            gen.Emit(OpCodes.Stelem_Ref);
        }

        // this.FakeMeta.Call(this, 'method.Name', types, args);
        gen.Emit(OpCodes.Ldarg_0);
        gen.Emit(OpCodes.Callvirt, metaGetter);
        gen.Emit(OpCodes.Ldarg_0);
        gen.Emit(OpCodes.Ldstr, method.Name);
        gen.Emit(OpCodes.Ldloc, types);
        gen.Emit(OpCodes.Ldloc, args);
        if (method.ReturnType != typeof(void))
        {
            gen.Emit(OpCodes.Callvirt, _ResultChainer.MakeGenericMethod(method.ReturnType));
        }
        else
        {
            gen.Emit(OpCodes.Callvirt, _VoidChainer);
        }

        // params[0..x] = ((OutRef)args[0..x]).Var;
        for (int i = 0; i < argInfos.Length; i++)
        {
            if (argInfos[i].ParameterType.IsByRef)
            {
                Type outRef = typeof(OutRef<>).MakeGenericType(argInfos[i].ParameterType.GetElementType()!);

                gen.Emit(OpCodes.Ldarg, i + 1);
                gen.Emit(OpCodes.Ldloc, args);
                gen.Emit(OpCodes.Ldc_I4, i);
                gen.Emit(OpCodes.Ldelem_Ref);
                gen.Emit(OpCodes.Castclass, outRef);
                gen.Emit(OpCodes.Ldfld, outRef.GetField(nameof(OutRef<Type>.Var))!);
                gen.Emit(OpCodes.Stind_Ref);
            }
        }

        gen.Emit(OpCodes.Ret);
    }

    /// <summary>Creates the constructor with the backing field for the meta provider.</summary>
    /// <param name="newType">Dynamic type being created.</param>
    /// <param name="parent">Base class inheriting from.</param>
    /// <returns>Info for the meta provider field.</returns>
    private static FieldBuilder SetupConstructor(TypeBuilder newType, Type parent)
    {
        ConstructorInfo? baseConstructor = parent
            .GetConstructors(_MemberFinder)
            .SingleOrDefault(c => c.GetParameters().Length == 0);

        FieldBuilder backingField = newType.DefineField(
            "_" + nameof(IFaked.FakeMeta), MetaType,
            FieldAttributes.Private | FieldAttributes.InitOnly);

        ConstructorBuilder constructor = newType.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.HasThis,
            [MetaType]);
        {
            // base();
            ILGenerator gen = constructor.GetILGenerator();
            if (baseConstructor != null)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Call, baseConstructor);
            }

            // this.m_FakeMeta = params[0];
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stfld, backingField);
            gen.Emit(OpCodes.Ret);
        }

        return backingField;
    }

    /// <summary>Hooks up the fake behavior mechanism for the new type.</summary>
    /// <param name="newType">Dynamic type being created.</param>
    /// <param name="backingField">Field holding the meta provider.</param>
    /// <returns>Info for the meta provider get hook.</returns>
    private static MethodInfo SetupFakeMetaProvider(TypeBuilder newType, FieldInfo backingField)
    {
        PropertyInfo propInfo = FakeType.GetProperty(nameof(IFaked.FakeMeta))!;
        MethodInfo getterInfo = propInfo.GetGetMethod()!;

        MethodBuilder getMetaMethod = newType.DefineMethod(
            nameof(IFaked) + "." + getterInfo.Name,
            getterInfo.Attributes & ~MethodAttributes.Abstract,
            MetaType,
            Type.EmptyTypes);
        {
            // return this._FakeMeta;
            ILGenerator gen = getMetaMethod.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, backingField);
            gen.Emit(OpCodes.Ret);
        }

        newType.DefineProperty(nameof(IFaked) + "." + propInfo.Name,
            propInfo.Attributes, MetaType, Type.EmptyTypes).SetGetMethod(getMetaMethod);

        newType.DefineMethodOverride(getMetaMethod, getterInfo);
        return getterInfo;
    }
}
