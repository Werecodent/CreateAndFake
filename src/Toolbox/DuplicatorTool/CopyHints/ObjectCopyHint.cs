using System.Reflection;
using CreateAndFake.Design;

namespace CreateAndFake.DuplicatorTool.CopyHints;

/// <summary>Handles cloning objects for <see cref="IDuplicator"/> .</summary>
public sealed class ObjectCopyHint : CopyHint
{
    /// <summary>Flags used to identify members.</summary>
    private const BindingFlags _MemberFlags
        = BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance;

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        object? result = Copy(source, duplicator);
        return (result != null)
            ? new(result)
            : CopyHintResult.None;
    }

    /// <inheritdoc cref="CopyHint{T}.CopyHint"/>
    private static object? Copy(object source, DuplicatorChainer duplicator)
    {
        object? dupe = CreateNew(source, duplicator);
        if (dupe == null)
        {
            return dupe;
        }
        duplicator.AddToHistory(source, dupe);

        foreach (FieldInfo field in source.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(f => !f.IsInitOnly && !f.IsLiteral))
        {
            field.SetValue(dupe, duplicator.Copy(field.GetValue(source)));
        }

        foreach (PropertyInfo property in source.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead && p.CanWrite))
        {
            property.SetValue(dupe, duplicator.Copy(property.GetValue(source)));
        }

        return dupe;
    }

    /// <summary>Creates an instance of <paramref name="source"/>'s <see cref="Type"/>.</summary>
    /// <param name="source">Object whose <see cref="Type"/> is to be created.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>The created instance.</returns>
    private static object? CreateNew(object source, DuplicatorChainer duplicator)
    {
        Type type = source.GetType();

        if (type.GetConstructor(Type.EmptyTypes) != null)
        {
            return Activator.CreateInstance(type);
        }
        else
        {
            PropertyInfo[] props = [.. type.GetProperties(_MemberFlags).Where(p => p.CanRead)];
            FieldInfo[] fields = [.. type.GetFields(_MemberFlags)];

            return type.GetConstructors(_MemberFlags)
                .Where(c => !c.IsPrivate)
                .OrderByDescending(c => c.GetParameters().Length)
                .Select(c => TryCreate(source, duplicator, c, props, fields))
                .FirstOrDefault(o => o != null);
        }
    }

    /// <summary>Attempts to create an instance using a <paramref name="constructor"/>.</summary>
    /// <param name="source">Object being cloned.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <param name="constructor">Constructor on <paramref name="source"/> to use.</param>
    /// <param name="props">Properties on <paramref name="source"/>.</param>
    /// <param name="fields">Fields on <paramref name="source"/>.</param>
    /// <returns>Null if failed; created instance otherwise.</returns>
    private static object? TryCreate(object source, DuplicatorChainer duplicator,
        ConstructorInfo constructor, IEnumerable<PropertyInfo> props, IEnumerable<FieldInfo> fields)
    {
        List<PropertyInfo> propList = [.. props];
        List<FieldInfo> fieldList = [.. fields];

        // Attempts to match members with parameters in the constructor.
        List<MemberInfo> matchedMembers = [];
        foreach (ParameterInfo param in constructor.GetParameters())
        {
            PropertyInfo[] potentialProps = [.. propList.Where(p => p.PropertyType.Inherits(param.ParameterType))];
            if (potentialProps.Length != 0)
            {
                PropertyInfo? directPropMatch = potentialProps.FirstOrDefault(
                    p => p.Name.Equals(param.Name, StringComparison.OrdinalIgnoreCase));
                if (directPropMatch != null)
                {
                    _ = propList.Remove(directPropMatch);
                    matchedMembers.Add(directPropMatch);
                }
                else
                {
                    _ = propList.Remove(potentialProps.First());
                    matchedMembers.Add(potentialProps.First());
                }
                continue;
            }

            FieldInfo[] potentialFields = [.. fieldList.Where(f => f.FieldType.Inherits(param.ParameterType))];
            if (potentialFields.Length != 0)
            {
                FieldInfo? directFieldMatch = potentialFields.FirstOrDefault(
                    f => f.Name.Equals(param.Name, StringComparison.OrdinalIgnoreCase));
                if (directFieldMatch != null)
                {
                    _ = fieldList.Remove(directFieldMatch);
                    matchedMembers.Add(directFieldMatch);
                }
                else
                {
                    _ = fieldList.Remove(potentialFields.First());
                    matchedMembers.Add(potentialFields.First());
                }
                continue;
            }

            return null;
        }

        return constructor.Invoke([.. matchedMembers.Select(m => CopyMember(m, source, duplicator))]);
    }

    /// <summary>Copies the value of <paramref name="member"/> on <paramref name="source"/>.</summary>
    /// <param name="member">Property or field to copy.</param>
    /// <param name="source">Instance containing the member.</param>
    /// <param name="duplicator">Duplicator handling the cloning.</param>
    /// <returns>The duplicate object.</returns>
    private static object? CopyMember(MemberInfo member, object source, DuplicatorChainer duplicator)
    {
        if (member is PropertyInfo prop)
        {
            return duplicator.Copy(prop.GetValue(source));
        }
        else
        {
            return duplicator.Copy(((FieldInfo)member).GetValue(source));
        }
    }
}
